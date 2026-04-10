# 13_decision_log.md

## 1. Назначение документа

Документ фиксирует принятые решения V1, чтобы они не переоткрывались неформально в чатах, коде или вспомогательных заметках.

---

## 2. Decision log

### D-001. V1 roles are fixed
В V1 существуют только:
- `Operator`
- `TechnicalSpecialist`
- `ServiceOwner`
- `Administrator`

Новые роли не создаются.

### D-002. Authoritative status source
`current_status` определяется из отдельного status source, а не из Prometheus.

### D-003. Only Maintenance is manual
Ручной прикладной статус V1 — только `Maintenance`.

### D-004. Service links are part of MVP
Базовые service links входят в MVP, а Full V1 добавляет richer presentation/admin helpers.

### D-005. Backend-only admin flows are allowed in MVP
До появления UI для Users/Roles используются:
- `controlled seed`
- `internal admin command`
- `migration-assisted assignment`
- backend-only controlled runbook

### D-006. Backend-only admin flows require audit
Все backend-only administrative changes обязаны оставлять audit trail.

### D-007. service_links model is mandatory
`service_links` — обязательная сущность V1.

### D-008. Link edit scope
`ServiceOwner` может редактировать только:
- `runbook`
- `docs`
- `dashboard`

`logs`, `alerts`, `other` — admin only.

### D-009. 30d metrics are Full V1 only
`30d` range допустим только во Full V1 и только при достаточном retention.

### D-010. Unhealthy integration threshold lives outside foundation
Точные thresholds фиксируются в `10_non_functional_requirements.md` и `11_error_handling_and_degraded_mode.md`.

### D-011. Exact contracts live in 02_solution_design.md
JSON/status-source contract и validation shapes не живут в foundation.

### D-012. Bootstrap first admin is one-time
Bootstrap-admin действует до появления первого active admin и затем должен быть отключён.

---

## 3. Change control

Изменение любого решения из этого журнала требует:
1. обновления `01_project_foundation.md`;
2. обновления соответствующих дочерних документов;
3. обновления реализации.

---

## 4. Consistency rule

Этот документ обязан быть синхронизирован с `01_project_foundation.md`.
