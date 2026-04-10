# 11_error_handling_and_degraded_mode.md

## 1. Назначение документа

Документ фиксирует:
- taxonomy ошибок;
- degraded behavior;
- policy для unhealthy integration;
- retry/error thresholds в operational-logic контексте.

Этот документ описывает error/degraded semantics системы.

Он не является:
- основным policy-документом по retention/session/security baseline;
- procedural runbook document;
- local setup guide.

Если документ конфликтует с `01_project_foundation.md`, foundation имеет приоритет.

---

## 2. Граница ответственности документа

### 2.1. Что живёт здесь

В этом документе фиксируются:
- типы ошибок интеграций;
- degraded behavior для status source и metrics integration;
- правила перехода в degraded/unknown-safe state;
- сигналы для escalation/diagnostics;
- operational interpretation failures.

### 2.2. Что живёт в других документах

- thresholds, retention, session/re-auth и policy-level baseline живут в `10_non_functional_requirements.md`;
- runbook execution, verification и remediation steps живут в `12_run_guide.md`;
- environment and local run concerns живут в `14_environment_and_run.md`.

---

## 3. Error classes for status source

Для V1 различаются:
- `transport_error`
- `partial_response`
- `invalid_payload`
- `unknown_external_status`
- `invalid_timestamp`

---

## 4. Error classes for metrics integration

Для V1 различаются:
- `metrics_transport_error`
- `metrics_timeout`
- `metrics_partial_data`
- `metrics_no_data`
- `metrics_query_error`

Metrics failures не должны ломать Overview / History / Timeline / Comments / Links.

---

## 5. Degraded behavior

### 5.1. Status source down

Если status source недоступен:
- пока stale threshold не превышен, используется last known status;
- после stale threshold сервис уходит в `Unknown`.

### 5.2. Partial response

При partial response:
- успешными считаются только сервисы с валидными данными;
- для остальных применяется stale-policy;
- интеграция получает degraded marker.

### 5.3. Invalid payload

Если payload невалиден:
- запись по сервису не считается доверенно обновлённой;
- last known status не затирается немедленно;
- далее применяется stale-policy;
- failure должен фиксироваться в diagnostics/technical logs.

### 5.4. Unknown external status

Если внешний статус пришёл, но не может быть интерпретирован:
- значение не маппится в новый прикладной статус;
- сервис не считается успешно обновлённым по этому item;
- для сервиса применяется stale-policy;
- проблема должна маркироваться как validation/integration issue.

### 5.5. Prometheus down

Если Prometheus недоступен:
- service overview остаётся доступным;
- metrics section показывает degraded state;
- `current_status` не меняется только из-за падения Prometheus.

---

## 6. Retry policy

### 6.1. Interactive user flows

- max retry = `1`

### 6.2. Internal polling

- допускается controlled retry по integration policy;
- retry не должен приводить к бесконечным блокировкам.

Policy-level baseline и timeout values синхронизируются с `10_non_functional_requirements.md`.

---

## 7. Unhealthy integration policy

Интеграция получает `is_healthy = false`, если:
- `3` подряд polling cycles завершились ошибкой;
- либо `last_successful_sync_at` старше `15 минут` для обязательной интеграции;
- либо `partial_response` повторяется `3` раза подряд;
- либо repeated invalid payload errors делают интеграцию непригодной для доверенного чтения.

Возврат к `healthy` допустим только после успешного цикла, который восстанавливает доверенное состояние интеграции.

---

## 8. Escalation triggers

Следующие условия должны попадать в admin/data-quality и operational diagnostics:
- integration unhealthy
- stale status beyond threshold
- repeated payload validation errors
- repeated duplicate/invalid link validation failures
- missing required integration keys у active services

---

## 9. Correlation and diagnostics

Для integration failures должны фиксироваться:
- error class
- correlation id
- attempt timestamp
- integration name
- user-visible safe state
- admin-visible detailed state

---

## 10. Safe user-visible behavior

При любой из описанных ошибок UI должен оставаться:
- безопасным;
- без stack trace;
- без раскрытия секретов;
- без raw internal payload;
- с role-appropriate diagnostics subset.

Если пользователю нужен видимый marker проблемы, он должен описывать безопасный operational смысл, а не внутренние технические детали.

---

## 11. Remediation boundary

Этот документ описывает **когда и почему** система попадает в degraded/error state.

Пошаговые действия **что делать оператору или trusted admin** после такой деградации закрепляются в `12_run_guide.md`.

---

## 12. Consistency rule

Этот документ обязан быть синхронизирован с:
- `01_project_foundation.md`
- `10_non_functional_requirements.md`
- `12_run_guide.md`
- `14_environment_and_run.md`

Если возникает конфликт, `01_project_foundation.md` имеет приоритет.
