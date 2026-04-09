# 12_run_guide.md

## 1. Назначение документа

Документ фиксирует operational-порядок запуска backend-only administrative flows, verification steps и минимальные эксплуатационные требования для V1.

Если документ конфликтует с `01_project_foundation.md`, foundation имеет приоритет.

---

## 2. Допустимые backend-only administrative flows

Для V1 допустимы только:
- `controlled seed`
- `internal admin command`
- `migration-assisted assignment`
- эквивалентный backend-only controlled operational runbook

Использование иных ad hoc способов административного изменения запрещено.

---

## 3. Кто имеет право запускать runbooks

Backend-only administrative runbooks могут запускать только:
- административная роль;
- ограниченный доверенный эксплуатационный персонал;
- сотрудники, явно включённые в operational access list окружения.

---

## 4. Общий порядок выполнения backend-only admin action

1. Подтвердить основание для изменения.
2. Проверить, что actor имеет право запускать соответствующий runbook.
3. Зафиксировать target entity и intended change.
4. Выполнить backend-only action через допустимый механизм.
5. Выполнить post-action verification.
6. Зафиксировать результат выполнения.
7. При ошибке или отсутствии audit trail — выполнить escalation/remediation.

---

## 5. Обязательный post-action verification

Каждый backend-only administrative action обязан завершаться проверкой:

### 5.1. Business effect verification
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

### 5.2. Audit verification
Проверить наличие audit record по:
- actor
- target entity
- action type
- approximate execution time

### 5.3. Diagnostics verification
Если действие влияет на diagnostics/configuration:
- проверить, что состояние UI/diagnostics стало согласованным;
- при необходимости проверить обновлённые timestamps или исчезновение configuration issue.

---

## 6. Failure handling

Если business effect применился, но audit record отсутствует:
- действие считается **failed administrative action**;
- требуется escalation;
- требуется remediation или controlled rollback;
- runbook не считается завершённым успешно.

Если action не применился:
- фиксируется ошибка;
- повторное выполнение допускается только по правилам operational control.

---

## 7. Minimum logging for runbooks

Каждый запуск runbook должен фиксировать:
- кто запускал;
- когда запускал;
- какой runbook использовался;
- какую сущность меняли;
- краткий смысл изменения;
- success/failure outcome;
- correlation/reference id, если применимо.

---

## 8. Security rules for runbooks

- запуск только в доверенном серверном контуре;
- не использовать личные или неуправляемые каналы выполнения;
- секреты и sensitive payloads не должны логироваться в открытом виде;
- результаты runbook не должны передаваться в незащищённые каналы.

---

## 9. Consistency rule

Этот документ обязан быть синхронизирован с:
- `01_project_foundation.md`
- `10_non_functional_requirements.md`
- `11_error_handling_and_degraded_mode.md`

Если возникает конфликт, `01_project_foundation.md` имеет приоритет.
