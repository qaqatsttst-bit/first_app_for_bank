# 12_run_guide.md

## 1. Назначение документа

Документ фиксирует operational-порядок запуска backend-only administrative flows, verification steps и минимальные эксплуатационные требования для V1.

Этот документ является **admin/runbook document**.

Он не является:
- local setup guide;
- developer environment guide;
- инструкцией по PostgreSQL bootstrap для разработки;
- полным документом по environment configuration.

Если документ конфликтует с `01_project_foundation.md`, foundation имеет приоритет.

---

## 2. Граница ответственности документа

### 2.1. Что живёт здесь

В этом документе фиксируются:
- допустимые backend-only administrative flows;
- кто имеет право их запускать;
- preconditions выполнения;
- execution order;
- post-action verification;
- remediation/escalation expectations;
- minimal evidence/logging expectations.

### 2.2. Что живёт в других документах

- local run, environment variables, migrations, bootstrap и seed-подготовка живут в `14_environment_and_run.md`;
- threshold/policy-level baseline живёт в `10_non_functional_requirements.md`;
- taxonomy деградаций и интеграционных ошибок живёт в `11_error_handling_and_degraded_mode.md`.

---

## 3. Out of scope

Следующие темы не должны описываться здесь как основной источник истины:
- локальный запуск приложения;
- настройка dev environment;
- установка SDK/runtime;
- создание локальной БД для разработчика;
- OIDC environment bootstrap;
- локальные команды миграций как developer handbook;
- demo mode / seed mode как developer setup process.

---

## 4. Допустимые backend-only administrative flows

Для V1 допустимы только:
- `controlled seed`
- `internal admin command`
- `migration-assisted assignment`
- эквивалентный backend-only controlled operational runbook

Иные ad hoc способы административного изменения запрещены.

---

## 5. Кто имеет право запускать runbooks

Backend-only administrative runbooks могут запускать только:
- административная роль;
- ограниченный доверенный эксплуатационный персонал;
- сотрудники, явно включённые в operational access list окружения.

---

## 6. Preconditions for admin action

Перед запуском backend-only administrative action обязательно:
1. Подтвердить основание для изменения.
2. Проверить, что actor имеет право запускать соответствующий runbook.
3. Зафиксировать target entity.
4. Зафиксировать intended change.
5. Понять, какой audit trail должен быть создан.
6. Понять, какой business effect ожидается после выполнения.
7. Убедиться, что действие выполняется в доверенном серверном контуре.

Если хотя бы одно из этих условий не выполнено, runbook не должен запускаться как штатная операция.

---

## 7. Общий порядок выполнения backend-only admin action

1. Подтвердить основание для изменения.
2. Проверить полномочия actor.
3. Зафиксировать target entity и intended change.
4. Выполнить backend-only action через допустимый механизм.
5. Выполнить post-action verification.
6. Зафиксировать результат выполнения.
7. При ошибке или отсутствии audit trail — выполнить escalation/remediation.

---

## 8. Обязательный post-action verification

### 8.1. Business effect verification

Проверить, что изменение реально применилось:
- роль назначена/снята;
- owner изменён;
- category изменена;
- service activated/deactivated;
- metadata обновлена;
- integration key изменён;
- maintenance установлен/снят;
- link создан/изменён/деактивирован;
- comment redaction выполнен.

### 8.2. Audit verification

Проверить наличие audit record по:
- actor
- target entity
- action type
- approximate execution time

### 8.3. Diagnostics verification

Если действие влияет на diagnostics/configuration:
- проверить, что состояние UI/diagnostics стало согласованным;
- при необходимости проверить обновлённые timestamps или исчезновение configuration issue.

---

## 9. Failure handling

Если business effect применился, но audit record отсутствует:
- действие считается **failed administrative action**;
- требуется escalation;
- требуется remediation или controlled rollback;
- runbook не считается завершённым успешно.

Если action завершился частично:
- частичный результат должен быть явно зафиксирован;
- последующее состояние не должно маскироваться как success;
- при необходимости требуется controlled rollback или compensating action.

---

## 10. Minimal rollback / remediation expectations

Если backend-only administrative action завершился с отклонением от ожидаемого результата:
- должен существовать способ понять текущее фактическое состояние target entity;
- должна существовать процедура escalation к trusted admin/ops;
- отсутствие immediate rollback не отменяет обязательность фиксации инцидента выполнения;
- remediation должна быть audit-friendly и не выполняться скрыто вне доверенного контура.

Rollback допустим только:
- если он безопасен;
- если он не ломает исторические данные;
- если он не скрывает уже произошедшее административное воздействие.

---

## 11. Evidence to keep after execution

После выполнения runbook должно быть возможно восстановить:
- кто запускал;
- когда запускал;
- какой runbook использовался;
- какую сущность меняли;
- краткий смысл изменения;
- expected result;
- actual result;
- success/failure outcome;
- correlation/reference id, если применимо;
- наличие или отсутствие audit record.

---

## 12. Minimum logging for runbooks

Каждый запуск runbook должен фиксировать:
- кто запускал;
- когда запускал;
- какой runbook использовался;
- какую сущность меняли;
- краткий смысл изменения;
- success/failure outcome;
- correlation/reference id, если применимо.

---

## 13. Security rules for runbooks

- запуск только в доверенном серверном контуре;
- не использовать неуправляемые каналы выполнения;
- секреты и sensitive payloads не должны логироваться в открытом виде;
- результаты runbook не должны передаваться в незащищённые каналы.

---

## 14. Consistency rule

Этот документ обязан быть синхронизирован с:
- `01_project_foundation.md`
- `10_non_functional_requirements.md`
- `11_error_handling_and_degraded_mode.md`
- `14_environment_and_run.md`

Если возникает конфликт, `01_project_foundation.md` имеет приоритет.
