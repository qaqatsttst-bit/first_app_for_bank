# 03_implementation_plan.md

## 1. Назначение документа

Документ фиксирует фазовый план реализации V1.

## 2. Принципы выполнения

- Сначала закрываются foundation и child-doc decisions.
- Затем реализуются domain и application invariants.
- После этого — инфраструктура, UI и operational hardening.
- Код не должен переоткрывать уже закрытые решения foundation.

## 3. Фазы

### Phase 0 — Documentation baseline
- утвердить `01`;
- утвердить child docs;
- устранить противоречия между документами;
- зафиксировать decision log.

### Phase 1 — Core domain and data
- реализовать сущности `Service`, `Category`, `User`, `Role`, `ServiceOwnerRelation`;
- реализовать migrations;
- реализовать activation invariants;
- реализовать append-only comments/status history/timeline.

### Phase 2 — Identity and access
- OIDC login;
- local user bootstrap;
- local roles;
- owner-scoped authorization;
- bootstrap admin path.

### Phase 3 — Status source integration
- backend polling;
- mapping external-to-internal status;
- stale behavior;
- status history recording;
- diagnostics markers.

### Phase 4 — Metrics integration
- Prometheus integration;
- `24h` / `7d` ranges;
- applicability matrix;
- degraded metrics behavior.

### Phase 5 — UI MVP
- dashboard;
- catalog;
- service card;
- categories page;
- comments;
- timeline;
- service links.

### Phase 6 — Operational hardening
- audit coverage;
- admin backend-only flows;
- troubleshooting;
- run docs;
- verification;
- readiness validation.
