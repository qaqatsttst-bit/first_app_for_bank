# 09_pages_and_navigation.md

## 1. Назначение документа

Документ фиксирует page-behavior contract, page composition, UI states, обязательные markers и page-level поведение V1.

Этот документ определяет:
- какие страницы и секции существуют;
- какие section states обязательны;
- какие markers обязательны;
- что обязано быть видно обычным ролям;
- что должно быть доступно только административным ролям.

Этот документ не является pixel-perfect design specification и не фиксирует визуальные детали типа цветов, отступов и иконок, если они не влияют на прикладной смысл.

Если документ конфликтует с `01_project_foundation.md`, foundation имеет приоритет.

---

## 2. MVP pages

- Главная страница
- Каталог сервисов
- Карточка сервиса
- Страница категорий

## 3. Full V1 pages

- Users
- Roles
- Minimal admin section
- Admin/data-quality view
- Расширенные представления управления links и diagnostics

---

## 4. Service card sections

Карточка включает:
- Overview
- Metrics / Dynamics
- Status History
- Comments
- Timeline
- Links / Diagnostics

Порядок секций может отличаться в UI, но прикладной состав секций должен сохраняться.

---

## 5. Overview behavior

### 5.1. Overview mandatory fields

Overview карточки должен показывать минимум:
- `current_status`
- `criticality`
- `service_type`
- владельцев
- описание
- `last_successful_status_refresh_at`
- `last_successful_metrics_refresh_at`
- high-level integration health marker

### 5.2. Mandatory overview markers

Overview обязан уметь показывать:
- stale indicator
- configuration issue marker
- owner missing marker
- overdue maintenance marker
- last known status warning
- maintenance precedence marker, если это уместно по текущему состоянию

### 5.3. Owner missing state

Если owner отсутствует, Overview должен показывать явное состояние вида `owner is not assigned`, а не маскировать это под обычное empty state.

---

## 6. Metrics / Dynamics behavior

### 6.1. Supported ranges

Для MVP поддерживаются:
- `24h`
- `7d`

Для Full V1 может поддерживаться:
- `30d`, если retention позволяет.

### 6.2. Mandatory metric semantics

UI обязан различать:
- `Not applicable`
- `No data`
- `Unavailable`

### 6.3. Metrics degraded behavior

Если Prometheus недоступен:
- Overview остаётся доступным;
- Status History остаётся доступной;
- Comments остаются доступными;
- Timeline остаётся доступной;
- Links / Diagnostics остаются доступными;
- Metrics section показывает degraded / unavailable state.

### 6.4. Partial metrics state

Если доступна только часть обязательных метрик:
- доступные метрики показываются;
- недоступные явно маркируются;
- вся карточка не должна считаться сломанной по умолчанию.

---

## 7. Status History behavior

### 7.1. History fields

История статусов должна показывать минимум:
- старый статус
- новый статус
- время
- источник
- пользователя, если изменение было ручным
- комментарий, если он был

### 7.2. Empty state

Если исторических записей нет, должен показываться явный `no history state`, а не общий page error.

---

## 8. Comments behavior

### 8.1. Comment list

Комментарии отображаются в отдельной секции карточки сервиса.

### 8.2. Redacted comments

Если комментарий был redacted/corrected по административному сценарию:
- обычный UI должен показывать marker `Comment redacted by administrator`;
- исходное содержание обычным ролям не показывается;
- redaction не должен выглядеть как простое отсутствие комментария.

### 8.3. Empty state

Если комментариев нет, должен показываться отдельный empty state секции comments.

---

## 9. Timeline behavior

### 9.1. Timeline events

Timeline должна показывать как минимум:
- `problem_confirmed`
- `problem_confirmation_revoked`
- `maintenance_started`
- `maintenance_ended`

### 9.2. Empty state

Если timeline пуст, показывается отдельный empty state timeline, а не общее пустое состояние страницы.

---

## 10. Links / Diagnostics behavior

### 10.1. Service links block

Блок service links входит в карточку сервиса уже в MVP.

Показывается:
- `title`
- `link_type`
- `url`

Отображаются только active links.

### 10.2. Empty links state

Если links нет, карточка показывает явный empty state:

`Служебные ссылки не настроены`

### 10.3. Invalid links

В MVP UI не выполняет автоматическую live-проверку “мертвая/живая ссылка”.

Обычная карточка отображает сохранённую ссылку как есть.

Admin/data-quality view должен показывать invalid link issues, если они вычислены backend validation.

### 10.4. Duplicate links

UI не должен позволять сохранить duplicate active link, если backend вернул validation error.

### 10.5. Diagnostics taxonomy

- **Service links** — служебные ссылки, относящиеся к конкретному сервису;
- **Per-service diagnostics** — `last_successful_status_refresh_at`, `last_successful_metrics_refresh_at`, stale indicators, configuration issues и service-level markers;
- **Integration-level diagnostics** — `integration_sync_state`, `last_attempt_at`, `last_error_message`, `correlation_id`, `is_healthy`;
- **Role-safe diagnostics subset** — безопасный поднабор, видимый всем ролям, имеющим доступ к карточке;
- **Admin diagnostics** — расширенный набор, доступный только admin.

### 10.6. Visibility boundary

Обычная карточка сервиса должна показывать только role-safe diagnostics subset.

Admin-only diagnostics не должны раскрываться обычным ролям через progressive disclosure или скрытые технические поля.

### 10.7. Link edit visibility

Редактирование:
- `ServiceOwner` — только `runbook`, `docs`, `dashboard` собственного сервиса;
- `Administrator` — все типы links;
- `Operator` и `TechnicalSpecialist` — без edit actions.

Owner-scoped edit actions должны отображаться только если одновременно соблюдены:
- активная роль `ServiceOwner`;
- ownership relation к конкретному сервису.

---

## 11. General UI states

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

---

## 12. Catalog behavior

Каталог поддерживает:
- поиск по `name`
- фильтрацию по category
- фильтрацию по status
- фильтрацию по criticality
- AND-combination filters

Draft не показывается в основном каталоге.

---

## 13. Main page behavior

Главная страница показывает:
- summary counts
- problem services
- maintenance services
- stale / unknown services
- integration issues
- services without owner

Manual refresh обязателен.

### 13.1. Services without owner block

Блок `services without owner` должен интерпретироваться как data-quality/governance block, а не как основной operational incident block.

### 13.2. Problem prioritization boundary

Блок `services without owner` не должен конкурировать визуально с основными problem lists (`Down`, `Degraded`, `Unknown`).

---

## 14. Degraded UI rules

### 14.1. Metrics failure

Если Prometheus недоступен:
- Overview/History/Comments/Timeline/Links остаются доступны;
- Metrics section показывает degraded state.

### 14.2. Status source temporary failure

Если source временно недоступен, но stale threshold не превышен:
- показывается last known status;
- отображается warning;
- источник проблемы должен выглядеть как data freshness/integration issue, а не как окончательное подтверждение деградации самого сервиса.

### 14.3. Missing mapping

Если нет integration keys:
- показывается configuration issue;
- это не маскируется под no data.

### 14.4. Unknown behavior

Если сервис ушёл в `Unknown`, UI должен позволять отличать хотя бы на уровне marker-смысла:
- stale data
- source unavailable
- invalid payload / cannot interpret status

Exact wording может быть определён позже, но сам explanatory marker обязателен.

---

## 15. Admin/data-quality view

Должен показывать минимум:
- draft без category
- draft в `Uncategorized`
- category issues
- owner issues
- missing integration keys
- duplicate/invalid links
- services with metrics wiring issue
- unhealthy integrations

---

## 16. Consistency rule

Этот документ обязан быть синхронизирован с:
- `01_project_foundation.md`
- `04_glossary.md`
- `08_data_model.md`
- `10_non_functional_requirements.md`
- `11_error_handling_and_degraded_mode.md`

Если возникает конфликт, `01_project_foundation.md` имеет приоритет.
