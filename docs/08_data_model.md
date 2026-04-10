# 08_data_model.md

## 1. Назначение документа

Документ фиксирует schema-level модель данных V1, обязательные поля сущностей, ограничения, storage-инварианты и activation rules.

## 2. Общие принципы

- PostgreSQL — основная transactional DB.
- Все даты и время хранятся в UTC.
- Исторически значимые сущности используют деактивацию, а не физическое удаление.
- Прикладные изменения, требующие аудита, должны порождать audit records.

## 3. Основные таблицы

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

## 4. Key invariants

### 4.1. Active service invariants
Active service обязан иметь:
- owner;
- валидную category;
- `service_key`;
- `slug`;
- `status_source_key`;
- `metrics_source_key`;
- `criticality`;
- `service_type`.

### 4.2. Draft service rules
Draft service может временно не иметь:
- owner;
- category;
- integration keys.

### 4.3. Uncategorized rule
Active service не может быть активирован в `Uncategorized`.

### 4.4. Owner-scoped authorization boundary
Ownership relation сама по себе не заменяет прикладную роль `ServiceOwner`.
