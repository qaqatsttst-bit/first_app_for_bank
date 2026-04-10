# 09_pages_and_navigation.md

## 1. Назначение документа

Документ фиксирует page-behavior contract, page composition, section states, обязательные markers и role-aware visibility V1.

## 2. MVP pages

В MVP обязательны:
- Главная страница
- Каталог сервисов
- Карточка сервиса
- Страница категорий

## 3. Full V1 pages

В Full V1 дополнительно могут входить:
- Users
- Roles
- Minimal admin section
- Admin/data-quality view
- расширенные diagnostics / links management views

## 4. Service card sections

Карточка включает:
- Overview
- Metrics / Dynamics
- Status History
- Comments
- Timeline
- Links / Diagnostics

## 5. Overview behavior

Overview карточки должен показывать минимум:
- `current_status`
- `criticality`
- `service_type`
- владельцев
- описание
- `last_successful_status_refresh_at`
- `last_successful_metrics_refresh_at`
- high-level integration health marker

## 6. Metrics / Dynamics behavior

Для MVP поддерживаются:
- `24h`
- `7d`

UI обязан различать:
- `Not applicable`
- `No data`
- `Unavailable`

Если Prometheus недоступен:
- Overview, History, Comments, Timeline и Links остаются доступны;
- Metrics section показывает degraded / unavailable state.

## 7. History / Comments / Timeline

История статусов должна показывать:
- старый статус
- новый статус
- время
- источник
- пользователя, если изменение было ручным
- комментарий, если он был

Redacted comments показываются как `Comment redacted by administrator`.

Timeline минимум:
- `problem_confirmed`
- `problem_confirmation_revoked`
- `maintenance_started`
- `maintenance_ended`

## 8. Links / Diagnostics behavior

Обычная карточка сервиса должна показывать только role-safe diagnostics subset.

Если нет integration keys:
- показывается configuration issue;
- это не маскируется под no data.

## 9. General UI states

Для ключевых страниц и секций должны быть предусмотрены:
- loading state
- empty state
- error state
- stale-data indicator
- no metrics state
- no history state
- no owner state
- permission denied state
- partial data state
- configuration issue state

## 10. Catalog and main page

Каталог поддерживает:
- поиск по `name`
- фильтрацию по category
- фильтрацию по status
- фильтрацию по criticality
- AND-combination filters

Главная страница показывает:
- summary counts
- problem services
- maintenance services
- stale / unknown services
- integration issues
- services without owner
