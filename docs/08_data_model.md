
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

### 3.2. categories
Обязательные поля:
- `id`
- `name`
- `is_active`
- `is_system_reserved`
- `created_at`
- `updated_at`

### 3.3. users
Обязательные поля:
- `id`
- `external_subject`
- `email`
- `display_name`
- `is_active`
- `created_at`
- `updated_at`

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

### 3.5. user_roles
Обязательные поля:
- `id`
- `user_id`
- `role_id`
- `assigned_at`
- `assigned_by`
- `is_active`

### 3.6. service_owners
Обязательные поля:
- `id`
- `service_id`
- `user_id`
- `is_primary`
- `is_active`
- `created_at`
- `updated_at`

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

### 3.10. service_timeline_events
Обязательные поля:
- `id`
- `service_id`
- `event_type`
- `event_time`
- `performed_by`
- `payload_json`

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
- невалидные URI.

### 4.3. Duplicate policy
Дубликатом считается active link с одинаковыми:
- `service_id`
- `link_type`
- `url`

Такие дубликаты запрещены.

### 4.4. Limits
Для одного сервиса:
- не более `20` active links суммарно;
- не более `5` active links одного `link_type`.

### 4.5. Delete policy
Физическое удаление links в обычных прикладных сценариях не используется.

Используется деактивация:
- `is_active = false`

### 4.6. Auditability
Следующие операции по links должны быть аудитируемыми:
- create
- update
- deactivate
- type change
- url change

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

---

## 6. Consistency rule

Этот документ обязан быть синхронизирован с:
- `01_project_foundation.md`
- `09_pages_and_navigation.md`
- `10_non_functional_requirements.md`

Если возникает конфликт, `01_project_foundation.md` имеет приоритет.
