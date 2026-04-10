
### `docs/03_implementation_plan.md`
```md
# 03_implementation_plan.md

## 1. Назначение документа

Документ фиксирует phased delivery plan для V1.

---

## 2. Этапы

### Этап 1. Foundation-aligned backend skeleton
- project structure: Domain / Application / Infrastructure / Web
- PostgreSQL schema baseline
- OIDC authentication
- local users / roles bootstrap
- audit baseline
- service / category / owner entities

### Этап 2. Status source integration
- bulk polling
- mapping by `status_source_key`
- status normalization
- stale-policy
- `integration_sync_state`

### Этап 3. Service card baseline
- Overview
- Status History
- Comments
- Timeline
- role-safe diagnostics subset

### Этап 4. Prometheus metrics
- canonical query templates
- `24h` and `7d`
- `Not applicable` vs `No data`
- degraded metrics mode

### Этап 5. Service links and data quality
- `service_links`
- owner/admin edit flows
- admin/data-quality markers
- invalid/duplicate/limit handling

### Этап 6. Full V1 enhancements
- Users / Roles UI
- minimal admin section
- extended links/diagnostics UX
- `30d` metrics if retention allows

---

## 3. Delivery principles

- foundation decisions first
- child docs next
- implementation after docs
- no silent deviation from foundation

---

## 4. Definition of done per phase

Phase complete only if:
- code exists
- docs aligned
- audit/security expectations met
- negative scenarios covered by tests

---

## 5. Risks

- contract drift between docs and code
- missing audit in backend-only flows
- metrics retention mismatch for `30d`
- role leakage in UI-only enforcement

---

## 6. Consistency rule

Этот документ обязан быть синхронизирован с `01_project_foundation.md`.
