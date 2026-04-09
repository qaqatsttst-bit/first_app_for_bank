# 11_error_handling_and_degraded_mode.md

## 1. Назначение документа

Документ фиксирует:
- taxonomy ошибок;
- degraded behavior;
- policy для unhealthy integration;
- retry/error thresholds.

Если документ конфликтует с `01_project_foundation.md`, foundation имеет приоритет.

---

## 2. Error classes for status source

Для V1 различаются:
- `transport_error`
- `partial_response`
- `invalid_payload`
- `unknown_external_status`
- `invalid_timestamp`

### 2.1. transport_error

Запрос не выполнен или не получен валидный transport-level response.

### 2.2. partial_response

Источник вернул данные только по части сервисов.

### 2.3. invalid_payload

Payload не соответствует ожидаемой contract-shape на уровне solution design.

### 2.4. unknown_external_status

Получен статус, который не маппится в foundation mapping.

### 2.5. invalid_timestamp

Получен timestamp, который не может быть корректно интерпретирован.

---

## 3. Error classes for metrics integration

Для V1 различаются:
- `metrics_transport_error`
- `metrics_timeout`
- `metrics_partial_data`
- `metrics_no_data`
- `metrics_query_error`

Metrics failures не должны ломать Overview / History / Timeline / Comments / Links.

---

## 4. Degraded behavior

### 4.1. Status source down

Если status source недоступен:
- пока stale threshold не превышен, используется last known status;
- после stale threshold сервис уходит в `Unknown`.

### 4.2. Partial response

При partial response:
- успешными считаются только сервисы с валидными данными;
- для остальных применяется stale-policy;
- интеграция получает degraded marker.

### 4.3. Prometheus down

Если Prometheus недоступен:
- service overview остаётся доступным;
- metrics section показывает degraded state;
- `current_status` не меняется только из-за падения Prometheus.

---

## 5. Retry policy

В интерактивных пользовательских сценариях:
- max retry = `1`

Во внутреннем polling:
- допускается controlled retry по integration policy;
- exact retry cadence определяется реализацией, но не должен приводить к бесконечным блокировкам.

---

## 6. Unhealthy integration policy

Интеграция получает `is_healthy = false`, если:
- `3` подряд polling cycles завершились ошибкой;
- либо `last_successful_sync_at` старше `15 минут` для обязательной интеграции;
- либо `partial_response` повторяется `3` раза подряд;
- либо repeated invalid payload errors делают интеграцию непригодной для доверенного чтения.

Возврат к `healthy` допустим только после успешного цикла, который восстанавливает доверенное состояние интеграции.

---

## 7. Escalation triggers

Следующие условия должны попадать в admin/data-quality и operational diagnostics:
- integration unhealthy
- stale status beyond threshold
- repeated payload validation errors
- repeated duplicate/invalid link validation failures, если они блокируют административные сценарии
- missing required integration keys у active services

---

## 8. Correlation and diagnostics

Для integration failures должны фиксироваться:
- error class
- correlation id
- attempt timestamp
- integration name
- user-visible safe state
- admin-visible detailed state

---

## 9. Consistency rule

Этот документ обязан быть синхронизирован с:
- `01_project_foundation.md`
- `10_non_functional_requirements.md`
- `12_run_guide.md`

Если возникает конфликт, `01_project_foundation.md` имеет приоритет.
