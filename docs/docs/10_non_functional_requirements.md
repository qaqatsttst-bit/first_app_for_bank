# 10_non_functional_requirements.md

## 1. Назначение документа

Документ фиксирует расширенные нефункциональные требования V1:
- performance;
- concurrency;
- retention;
- security settings;
- operational thresholds.

Если этот документ конфликтует с `01_project_foundation.md`, источником истины остаётся foundation.

---

## 2. Performance baseline

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

## 3. Operational thresholds

### 3.1. Status stale thresholds

- stale threshold для статуса: `5 минут`
- stale threshold для метрик: `10 минут`

### 3.2. Unhealthy integration threshold

Для V1 интеграция считается `unhealthy`, если выполняется хотя бы одно условие:
- `3` подряд неуспешных polling cycles;
- отсутствие `last_successful_sync_at` дольше `15 минут` для обязательной интеграции;
- repeated partial failures, приводящие к неполному обновлению более чем в `3` последовательных циклах.

Exact tuning может меняться по environment, но значение по умолчанию для V1 фиксируется здесь.

---

## 4. Logging retention and redaction

### 4.1. Audit log

- minimum online retention: `365 дней`
- append-only policy
- доступ только ограниченным административным ролям

### 4.2. Technical logs

- hot retention: `30 дней`
- archival policy определяется инфраструктурой окружения

### 4.3. Log redaction / masking

В открытом виде запрещено логировать:
- секреты
- токены
- credentials
- raw sensitive operational payloads
- значения, которые могут использоваться для повторной аутентификации

При необходимости такие значения должны быть:
- masked;
- partially redacted;
- полностью исключены из обычных technical logs.

---

## 5. Session and re-auth policy

- idle timeout обязателен;
- absolute session lifetime обязателен;
- re-auth policy для административно значимых сценариев обязана существовать;
- exact значения определяются deployment environment.

Рекомендуемый baseline:
- idle timeout: `30 минут`
- absolute session lifetime: `8 часов`

Если environment требует иные значения, они могут быть ужесточены, но не ослаблены без явного решения безопасности.

---

## 6. Administrative runbook access

Backend-only administrative runbooks могут выполняться только:
- административной ролью;
- ограниченным доверенным эксплуатационным персоналом;
- в доверенном серверном контуре.

Запуск таких runbooks с недоверенных рабочих мест или неавторизованным персоналом запрещён.

---

## 7. Post-action verification requirement

Любой backend-only administrative action должен завершаться verification step:
- проверить, что бизнес-изменение реально применилось;
- проверить, что создан audit record;
- проверить, что при необходимости обновились timestamps/diagnostics markers.

Отсутствие audit record после успешно применённого изменения считается failed administrative action и требует escalation/remediation.

---

## 8. Consistency rule

Этот документ обязан быть синхронизирован с:
- `01_project_foundation.md`
- `11_error_handling_and_degraded_mode.md`
- `12_run_guide.md`

Если возникает конфликт, `01_project_foundation.md` имеет приоритет.
