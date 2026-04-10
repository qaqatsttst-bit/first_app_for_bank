# 11_error_handling_and_degraded_mode.md

## 1. Назначение документа

Документ фиксирует taxonomy ошибок, degraded behavior и semantic policy для integration failures.

## 2. Error classes for status source

Для V1 различаются:
- `transport_error`
- `partial_response`
- `invalid_payload`
- `unknown_external_status`
- `invalid_timestamp`

## 3. Error classes for metrics integration

Для V1 различаются:
- `metrics_transport_error`
- `metrics_timeout`
- `metrics_partial_data`
- `metrics_no_data`
- `metrics_query_error`

## 4. Degraded behavior

### 4.1. Status source down
Если status source недоступен:
- пока stale threshold не превышен, используется last known status;
- после достижения threshold сервис уходит в `Unknown`.

### 4.2. Partial response
При partial response:
- успешными считаются только сервисы с валидными данными;
- для остальных применяется stale-policy;
- интеграция получает degraded marker.

### 4.3. Invalid payload
Если payload невалиден:
- запись по сервису не считается доверенно обновлённой;
- last known status не затирается немедленно;
- далее применяется stale-policy.

### 4.4. Prometheus down
Если Prometheus недоступен:
- service overview остаётся доступным;
- metrics section показывает degraded state;
- `current_status` не меняется только из-за падения Prometheus.

## 5. Retry semantics

Interactive flows должны использовать retry-budget из `10_non_functional_requirements.md`.

Internal polling может использовать controlled retry, но не должен приводить к бесконечным блокировкам.

## 6. Unhealthy integration semantics

Интеграция считается semantic-непригодной для trusted normal-read поведения, если:
- repeated failures исчерпали unhealthy threshold;
- repeated partial responses делают состояние недостоверным;
- repeated invalid payload / validation failures не позволяют доверенно использовать интеграцию как источник истины;
- длительное отсутствие trusted successful sync превышает thresholds.
