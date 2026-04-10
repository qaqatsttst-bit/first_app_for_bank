# 10_non_functional_requirements.md

## 1. Назначение документа

Документ фиксирует расширенные нефункциональные требования V1:
- performance;
- concurrency;
- retention;
- security settings;
- session/re-auth baseline;
- operational thresholds.

Этот документ фиксирует policy/threshold-level требования.

Он не является:
- taxonomy-документом по degraded/error behavior;
- procedural runbook document;
- инструкцией по локальному запуску.

Если документ конфликтует с `01_project_foundation.md`, foundation имеет приоритет.

---

## 2. Граница ответственности документа

### 2.1. Что живёт здесь

В этом документе фиксируются:
- количественные thresholds;
- retention rules;
- session/re-auth baseline;
- performance targets;
- concurrency/load assumptions;
- policy-level security expectations.

### 2.2. Что живёт в других документах

- error taxonomy, degraded behavior и unhealthy integration policy в деталях живут в `11_error_handling_and_degraded_mode.md`;
- operational execution steps, verification и remediation backend-only admin actions живут в `12_run_guide.md`;
- local setup, environment bootstrap и run instructions живут в `14_environment_and_run.md`.

### 2.3. Exact numeric source-of-truth rule

Все **точные numeric thresholds** V1 должны канонически фиксироваться именно в этом документе.

Если другой документ описывает stale/retry/unhealthy semantics, он не должен переопределять numeric values, уже зафиксированные здесь.

---

## 3. Performance baseline

Для V1 принимаются следующие baseline targets:
- до `500` active services;
- до `50` concurrent users;
- главная страница `p95 <= 2 секунды`;
- каталог `p95 <= 2 секунды`;
- карточка сервиса без metrics section `p95 <= 2 секунды`;
- manual refresh `<= 5 секунд`;
- timeout status source `5 секунд`;
- timeout Prometheus `5 секунд`;
- не более `1` retry в интерактивном пользовательском сценарии.

---

## 4. Operational thresholds

### 4.1. Status stale thresholds

- stale threshold для статуса: `5 минут`
- stale threshold для метрик: `10 минут`

### 4.2. Unhealthy integration threshold

Интеграция считается `unhealthy`, если:
- `3` подряд неуспешных polling cycles;
- или нет `last_successful_sync_at` дольше `15 минут` для обязательной интеграции;
- или partial failures повторяются более `3` последовательных циклов.

---

## 5. Logging retention and redaction

### 5.1. Audit log

- minimum online retention: `365 дней`
- append-only policy
- доступ только ограниченным административным ролям

### 5.2. Technical logs

- hot retention: `30 дней`
- archival policy определяется инфраструктурой окружения

### 5.3. Log redaction / masking

В открытом виде запрещено логировать:
- секреты
- токены
- credentials
- raw sensitive operational payloads
- данные, пригодные для повторной аутентификации

При необходимости такие значения должны быть masked/redacted или исключены.

---

## 6. Session and re-auth policy

- idle timeout обязателен;
- absolute session lifetime обязателен;
- re-auth policy для административно значимых сценариев обязана существовать.

Рекомендуемый baseline:
- idle timeout: `30 минут`
- absolute session lifetime: `8 часов`

---

## 7. Administrative runbook access

Backend-only administrative runbooks могут выполняться только:
- административной ролью;
- ограниченным доверенным эксплуатационным персоналом;
- в доверенном серверном контуре.

---

## 8. Post-action verification requirement

Любой backend-only administrative action должен завершаться verification step:
- проверить, что бизнес-изменение применилось;
- проверить, что создан audit record;
- проверить, что при необходимости обновились diagnostics markers.

Отсутствие audit record считается failed administrative action.

Пошаговый execution procedure и remediation flow закрепляются в `12_run_guide.md`, а не в этом документе.

---

## 9. Security baseline

Для V1 обязательны следующие нефункциональные security expectations:
- backend-only access к внешним интеграциям;
- хранение секретов только в backend configuration и/или secret storage окружения;
- недопустимость раскрытия admin-only diagnostics в обычном UI;
- серверное enforcement access rules независимо от поведения UI;
- конечные таймауты внешних интеграций;
- отсутствие бесконечных retries в интерактивных сценариях.

---

## 10. Reliability and degraded availability baseline

- Частичная недоступность одной интеграции не должна приводить к падению всего приложения.
- Metrics integration failure не должна ломать Overview / History / Comments / Timeline / Links.
- Временная деградация status source не должна немедленно стирать last known status до достижения stale threshold.
- Исчерпание retry budget должно приводить к деградированному состоянию, а не к зависанию UI.

Детальная taxonomy и exact degraded behavior фиксируются в `11_error_handling_and_degraded_mode.md` без переопределения numeric values из этого документа.

---

## 11. Consistency rule

Этот документ обязан быть синхронизирован с:
- `01_project_foundation.md`
- `11_error_handling_and_degraded_mode.md`
- `12_run_guide.md`
- `14_environment_and_run.md`

Если возникает конфликт, `01_project_foundation.md` имеет приоритет.
