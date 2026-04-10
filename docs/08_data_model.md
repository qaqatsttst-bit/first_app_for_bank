# 08_data_model.md

## 1. Назначение документа

Документ фиксирует schema-level модель данных V1, обязательные поля сущностей, ограничения, инварианты хранения и storage-правила.

Если документ конфликтует с `01_project_foundation.md`, источником истины остаётся foundation.

---

## 2. Общие принципы

- PostgreSQL — основная transactional DB.
- Все даты и время хранятся в UTC.
- Исторически значимые сущности используют деактивацию, а не физическое удаление.
- Прикладные изменения, требующие аудита по foundation, должны порождать audit records.
- Все role/governance ограничения должны поддерживаться не только приложением, но и schema-level constraints там, где это разумно и безопасно.
- V1 не вводит отдельную прикладную сущность `service_notes`; пользовательский operational context представляется через `service_comments`, `service_timeline_events` и audit semantics там, где это применимо.

---

## 3. Основные сущности

### 3.1. services

Обязательные поля:
- `id`
- `service_key`
- `slug`
- `name`
- `description`
- `current_status`
- `criticality`
- `service_type`
- `status_source_key`
- `metrics_source_key`
- `category_id`
- `is_active`
- `created_at`
- `updated_at`
- `last_successful_status_refresh_at`
- `last_successful_metrics_refresh_at`

Ограничения:
- `service_key` unique
- `slug` unique
- `name` not empty
- `current_status` ограничен значениями foundation
- `criticality` ограничен значениями foundation
- `service_type` ограничен значениями foundation

### 3.2. categories

Обязательные поля:
- `id`
- `name`
- `is_active`
- `is_system_reserved`
- `created_at`
- `updated_at`

Ограничения:
- `name` unique
- системная резервная категория определяется `is_system_reserved = true`, а не display name

### 3.3. users

Обязательные поля:
- `id`
- `external_subject`
- `email`
- `display_name`
- `is_active`
- `created_at`
- `updated_at`

Ограничения:
- `external_subject` unique
- `email` not empty

### 3.4. roles

Обязательные поля:
- `id`
- `code`
- `display_name`
- `is_active`
- `created_at`
- `updated_at`

Допустимые `code`:
- `Operator`
- `TechnicalSpecialist`
- `ServiceOwner`
- `Administrator`

Ограничения:
- `code` unique
- иных прикладных ролей в V1 не допускается

### 3.5. user_roles

Обязательные поля:
- `id`
- `user_id`
- `role_id`
- `assigned_at`
- `assigned_by`
- `is_active`

Ограничения:
- один и тот же active `user_id + role_id` не должен дублироваться

### 3.6. service_owners

Обязательные поля:
- `id`
- `service_id`
- `user_id`
- `is_primary`
- `is_active`
- `created_at`
- `updated_at`

Ограничения:
- не более одного active primary owner на сервис
- owner у active service обязателен по правилам foundation

### 3.7. service_links

Обязательные поля:
- `id`
- `service_id`
- `link_type`
- `title`
- `url`
- `sort_order`
- `is_active`
- `created_at`
- `updated_at`
- `created_by`
- `updated_by`

Допустимые `link_type`:
- `runbook`
- `dashboard`
- `logs`
- `alerts`
- `docs`
- `other`

Ограничения:
- `title` not empty
- `url` not empty
- `sort_order >= 0`
- active duplicate по `service_id + link_type + url` не допускается

### 3.8. status_history

Обязательные поля:
- `id`
- `service_id`
- `old_status`
- `new_status`
- `changed_at`
- `change_source`
- `changed_by`
- `comment`

Политика:
- append-only
- update/delete запрещены обычными прикладными сценариями

### 3.9. service_comments

Обязательные поля:
- `id`
- `service_id`
- `author_id`
- `body`
- `is_redacted`
- `redaction_reason`
- `created_at`
- `updated_at`

Политика:
- append-only
- обычное редактирование и удаление запрещены
- redaction/correction — только по foundation rules

### 3.10. service_timeline_events

Обязательные поля:
- `id`
- `service_id`
- `event_type`
- `event_time`
- `performed_by`
- `payload_json`

Допустимые `event_type` минимум:
- `problem_confirmed`
- `problem_confirmation_revoked`
- `maintenance_started`
- `maintenance_ended`

### 3.11. integration_sync_state

Обязательные поля:
- `id`
- `integration_name`
- `last_successful_sync_at`
- `last_attempt_at`
- `last_error_message`
- `is_healthy`
- `correlation_id`
- `updated_at`

Политика:
- одна запись может описывать текущее агрегированное состояние интеграции
- exact unhealthy threshold определяется не здесь, а в NFR/error-handling docs

---

## 4. Service links rules

### 4.1. Sorting

Ссылки сортируются:
1. по `sort_order` по возрастанию;
2. при равенстве — по `created_at` по возрастанию;
3. при равенстве — по `id` по возрастанию.

### 4.2. URL validation

Для V1 допустимы только абсолютные URL:
- `http://...`
- `https://...`

Недопустимы:
- относительные URL;
- пустые строки;
- невалидные URI;
- схемы, отличные от `http` и `https`.

### 4.3. Duplicate policy

Дубликатом считается active link с одинаковыми:
- `service_id`
- `link_type`
- `url`

Такие дубликаты запрещены.

Разные `title` при одинаковом `service_id + link_type + url` не считаются основанием для разрешения дубликата.

### 4.4. Limits

Для одного сервиса:
- не более `20` active links суммарно;
- не более `5` active links одного `link_type`.

### 4.5. Delete policy

Физическое удаление links в обычных прикладных сценариях не используется.

Используется деактивация:
- `is_active = false`

Если link уже фигурировал в аудируемых прикладных изменениях, физическое удаление запрещено.

### 4.6. Auditability

Следующие операции по links должны быть аудитируемыми:
- create
- update
- deactivate
- type change
- url change

### 4.7. Role-aware editing rule

Редактирование link types для V1:
- `ServiceOwner` может редактировать только `runbook`, `docs`, `dashboard` в пределах своего сервиса;
- `logs`, `alerts`, `other` редактируются только административной ролью.

---

## 5. Key invariants

Active service обязан иметь:
- owner
- валидную category
- `service_key`
- `slug`
- `status_source_key`
- `metrics_source_key`
- `criticality`
- `service_type`

Active service не может быть активирован в `Uncategorized`.

Draft service может временно не иметь:
- owner
- category
- integration keys

---

## 6. Data-quality markers

Следующие состояния должны быть технически выводимы как data-quality issues:
- active service без owner
- active service без валидной category
- active service без integration keys
- active service с invalid links
- duplicate link conflict
- owner/link edits вне разрешённого role scope

---

## 7. Рекомендуемые schema-level ограничения

Где это безопасно и не конфликтует с operational model, рекомендуется использовать:
- unique index на `services.service_key`
- unique index на `services.slug`
- unique index на `categories.name`
- partial unique index на active `user_roles (user_id, role_id)`
- partial unique index на active primary owner per service
- partial unique index на active `service_links (service_id, link_type, url)`

---

## 8. Consistency rule

Этот документ обязан быть синхронизирован с:
- `01_project_foundation.md`
- `04_glossary.md`
- `09_pages_and_navigation.md`
- `10_non_functional_requirements.md`

Если возникает конфликт, `01_project_foundation.md` имеет приоритет.
