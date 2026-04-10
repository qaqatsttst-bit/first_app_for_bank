# 03_implementation_plan.md

## 1. Назначение документа

Документ фиксирует phased delivery plan для V1.

Он не заменяет foundation и child docs, а описывает порядок поставки и зависимости между этапами.

---

## 2. Delivery prerequisites

До начала основной реализации должны существовать и быть согласованы:
- `01_project_foundation.md`
- `04_glossary.md`
- `05_role_permission_matrix.md`
- `10_non_functional_requirements.md`
- `14_environment_and_run.md`

Если эти документы не согласованы, реализация считается находящейся в зоне высокого риска document drift.

---

## 3. Этапы

### Этап 0. Documentation baseline and alignment
- foundation finalized enough for implementation start
- glossary finalized
- permission model clarified
- document ownership boundaries aligned
- run/setup boundaries aligned

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

### Этап 6. Run/setup hardening
- migrations flow stabilized
- bootstrap flow verified
- local/integration startup flow documented
- post-start verification checklist aligned with implementation

### Этап 7. Full V1 enhancements
- Users / Roles UI
- minimal admin section
- extended links/diagnostics UX
- `30d` metrics if retention allows

---

## 4. Delivery principles

- foundation decisions first
- child docs next
- implementation after docs
- no silent deviation from foundation
- exact numeric thresholds come only from `10_non_functional_requirements.md`
- owner-scoped access cannot be implemented without both role and ownership-scope semantics being respected

---

## 5. Definition of done per phase

Phase complete only if:
- code exists
- docs aligned
- audit/security expectations met
- negative scenarios covered by tests
- no unresolved contradiction with foundation and glossary remains

---

## 6. Risks

- contract drift between docs and code
- missing audit in backend-only flows
- metrics retention mismatch for `30d`
- role leakage in UI-only enforcement
- confusion between `ServiceOwner` role and ownership relation
- duplicated numeric thresholds across documents
- setup/run docs drifting away from executable startup artifacts near code

---

## 7. Consistency rule

Этот документ обязан быть синхронизирован с:
- `01_project_foundation.md`
- `04_glossary.md`
- `14_environment_and_run.md`

Если возникает конфликт, `01_project_foundation.md` имеет приоритет.
