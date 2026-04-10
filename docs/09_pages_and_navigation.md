# 09_pages_and_navigation.md

## 1. Назначение документа

Документ фиксирует page composition, UI states, пользовательские сценарии и page-level поведение V1.

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

---

## 5. Service links block

### 5.1. MVP behavior
Блок service links входит в карточку сервиса уже в MVP.

Показывается:
- `title`
- `link_type`
- `url`

Отображаются только active links.

### 5.2. Full V1 behavior
Во Full V1 допускаются:
- richer presentation;
- visual grouping;
- sort/edit helpers;
- admin/data-quality helpers;
- diagnostics enrichment around links.

### 5.3. Visibility
Все роли, имеющие доступ к карточке сервиса, видят service links.

Редактирование:
- `ServiceOwner` — только `runbook`, `docs`, `dashboard` собственного сервиса;
- `Administrator` — все типы links;
- `Operator` и `TechnicalSpecialist` — без edit actions.

### 5.4. Empty state
Если links нет, карточка показывает явный empty state:

`Служебные ссылки не настроены`

### 5.5. Invalid links
В MVP UI не выполняет автоматическую live-проверку “мертвая/живая ссылка”.

Обычная карточка отображает сохранённую ссылку как есть.

Admin/data-quality view должен показывать invalid link issues, если они вычислены backend validation.

### 5.6. Duplicate links
UI не должен позволять сохранить duplicate active link, если backend вернул validation error.

---

## 6. Diagnostics taxonomy

- **Service links** — служебные ссылки, относящиеся к конкретному сервису;
- **Per-service diagnostics** — `last_successful_status_refresh_at`, `last_successful_metrics_refresh_at`, stale indicators, configuration issues и service-level markers;
- **Integration-level diagnostics** — `integration_sync_state`, `last_attempt_at`, `last_error_message`, `correlation_id`, `is_healthy`;
- **Role-safe diagnostics subset** — безопасный поднабор, видимый всем ролям, имеющим доступ к карточке;
- **Admin diagnostics** — расширенный набор, доступный только admin.

---

## 7. General UI states

Для ключевых страниц должны быть предусмотрены:
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

## 8. Catalog behavior

Каталог поддерживает:
- поиск по `name`
- фильтрацию по category
- фильтрацию по status
- фильтрацию по criticality
- AND-combination filters

Draft не показывается в основном каталоге.

---

## 9. Main page behavior

Главная страница показывает:
- summary counts
- problem services
- maintenance services
- stale / unknown services
- integration issues
- services without owner

Manual refresh обязателен.

---

## 10. Degraded UI rules

### 10.1. Metrics failure
Если Prometheus недоступен:
- Overview/History/Comments/Timeline/Links остаются доступны;
- Metrics section показывает degraded state.

### 10.2. Status source temporary failure
Если source временно недоступен, но stale threshold не превышен:
- показывается last known status;
- отображается warning.

### 10.3. Missing mapping
Если нет integration keys:
- показывается configuration issue;
- это не маскируется под no data.

---

## 11. Admin/data-quality view

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

## 12. Consistency rule

Этот документ обязан быть синхронизирован с:
- `01_project_foundation.md`
- `08_data_model.md`
- `10_non_functional_requirements.md`
- `11_error_handling_and_degraded_mode.md`

Если возникает конфликт, `01_project_foundation.md` имеет приоритет.
