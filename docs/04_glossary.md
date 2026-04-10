# 04_glossary.md

## 1. Назначение документа

Документ фиксирует канонический glossary терминов V1.

Его цель:
- унифицировать терминологию между foundation, child docs и реализацией;
- убрать неоднозначность трактовок;
- не допустить повторного переопределения уже согласованных понятий в коде, UI и вспомогательных заметках.

Если термин определён здесь и в дочернем документе по-разному, приоритет имеет:
1. `01_project_foundation.md` для product/domain-level decisions;
2. настоящий glossary для канонического словаря терминов;
3. профильный дочерний документ для implementation/detail-level трактовки без изменения смысла термина.

---

## 2. Общие правила использования терминов

- Один и тот же термин в документации и коде должен использоваться в одном и том же смысле.
- Если для понятия уже существует канонический термин, не следует вводить рядом альтернативные формулировки без явного обновления glossary.
- Glossary не создаёт новые product rules сам по себе; он только фиксирует значения терминов, уже вытекающих из foundation и согласованных child docs.
- Если тема не имеет канонического термина, она считается незакрытой до явного добавления в glossary или foundation.

---

## 3. Основные доменные термины

### 3.1. Service

`Service` — основная прикладная сущность системы, представляющая конкретный наблюдаемый платёжный сервис, интеграционную точку, внешнее направление или провайдера, за состоянием которого ведётся operational monitoring context.

### 3.2. Category

`Category` — прикладная сущность для группировки сервисов в каталоге и UI.

### 3.3. User

`User` — локальная прикладная запись сотрудника внутри системы, создаваемая и используемая после успешной внешней аутентификации.

### 3.4. Role

`Role` — предопределённая прикладная роль V1, определяющая набор разрешённых действий пользователя.

Для V1 допустимы только:
- `Operator`
- `TechnicalSpecialist`
- `ServiceOwner`
- `Administrator`

### 3.5. ServiceOwner role

`ServiceOwner` как роль — одна из четырёх фиксированных прикладных ролей V1.

Эта роль определяет тип owner-scoped действий, которые пользователь **может** выполнять при наличии нужного service scope.

### 3.6. Ownership relation / service ownership

`Ownership relation` — связь между пользователем и конкретным сервисом, обычно представленная сущностью `service_owners`.

Эта связь определяет, **к какому именно сервису** применим owner-scope.

### 3.7. ServiceOwner access semantics

Owner-scoped access в V1 требует одновременно:
- активную роль `ServiceOwner`;
- активную ownership relation к конкретному сервису.

Role без ownership relation не даёт owner-scoped прав.

Ownership relation без роли `ServiceOwner` не даёт owner-scoped прав.

### 3.8. ServiceComment

`ServiceComment` — append-only служебный комментарий по сервису.

### 3.9. ServiceTimelineEvent

`ServiceTimelineEvent` — прикладное событие в timeline сервиса, фиксирующее значимое operational/admin действие.

### 3.10. StatusHistory

`StatusHistory` — неизменяемая историческая запись о смене прикладного статуса сервиса.

### 3.11. ServiceLink

`ServiceLink` — служебная ссылка, относящаяся к конкретному сервису.

Базовые `link_type` V1:
- `runbook`
- `dashboard`
- `logs`
- `alerts`
- `docs`
- `other`

### 3.12. IntegrationSyncState

`IntegrationSyncState` — агрегированное текущее состояние конкретной интеграции как источника данных, а не per-service history.

---

## 4. Идентификаторы и ключи

### 4.1. service_key

`service_key` — основной внутренний уникальный прикладной идентификатор сервиса в системе.

### 4.2. slug

`slug` — уникальный идентификатор маршрутизации UI для сервиса.

### 4.3. status_source_key

`status_source_key` — внешний ключ сопоставления сервиса с авторитетным источником состояния.

### 4.4. metrics_source_key

`metrics_source_key` — внешний ключ сопоставления сервиса с источником метрик.

---

## 5. Статусы и статусная модель

### 5.1. current_status

`current_status` — текущее нормализованное прикладное состояние сервиса внутри системы.

### 5.2. external_status

`external_status` — статус, полученный из внешнего авторитетного status source до внутренней нормализации.

### 5.3. OK

`OK` — сервис доступен и работает в ожидаемых пределах.

### 5.4. Degraded

`Degraded` — сервис работает, но качество или показатели ухудшены.

### 5.5. Down

`Down` — сервис недоступен или фактически не выполняет основную функцию.

### 5.6. Unknown

`Unknown` — система не может надёжно определить текущее состояние сервиса.

### 5.7. Maintenance

`Maintenance` — согласованное техническое обслуживание, отображаемое как текущий прикладной статус до ручного завершения режима обслуживания.

### 5.8. authoritative status source / status source

`Authoritative status source` или `status source` — внешний источник истины для `current_status`, к которому backend обращается server-to-server.

### 5.9. polling

`Polling` — фоновое периодическое получение данных backend-слоем из внешнего источника.

### 5.10. stale

`Stale` — состояние данных, чья актуальность вышла за допустимый временной порог.

### 5.11. stale threshold

`Stale threshold` — временной порог, после превышения которого данные считаются устаревшими.

### 5.12. last known status

`Last known status` — последнее успешно подтверждённое состояние сервиса, которое ещё может временно отображаться до превышения stale threshold.

---

## 6. Диагностика и качество данных

### 6.1. diagnostics

`Diagnostics` — набор диагностических признаков и полей, помогающих понять качество, свежесть и источник данных по сервису или интеграции.

### 6.2. role-safe diagnostics subset

`Role-safe diagnostics subset` — безопасный поднабор diagnostics, доступный всем ролям, имеющим право открыть карточку сервиса.

### 6.3. admin diagnostics

`Admin diagnostics` — расширенный набор diagnostics, доступный только административным ролям и административным представлениям.

### 6.4. configuration issue

`Configuration issue` — прикладная проблема настройки сервиса или интеграции, например:
- missing integration keys;
- invalid category;
- invalid link setup;
- metrics wiring problem.

### 6.5. data-quality violation

`Data-quality violation` — нарушение целостности или обязательных прикладных инвариантов, которое не должно скрываться как обычное empty/no-data состояние.

Примеры:
- active service без owner;
- active service без валидной category;
- duplicate active links;
- missing required integration keys.

### 6.6. unhealthy integration

`Unhealthy integration` — состояние интеграции, при котором её данные больше не считаются доверенно пригодными для нормальной operational работы по thresholds, зафиксированным в `10_non_functional_requirements.md`.

---

## 7. Жизненный цикл сервиса

### 7.1. Draft

`Draft` — сервисная запись, которая ещё не обязана удовлетворять всем правилам active service.

### 7.2. Active

`Active` — рабочая сервисная запись, которая обязана удовлетворять обязательным прикладным инвариантам V1.

### 7.3. Uncategorized

`Uncategorized` — системная резервная категория.

Для draft она допустима, для active service — не считается нормальным рабочим состоянием.

---

## 8. Метрики и их UI-семантика

### 8.1. availability

`availability` — метрика доступности сервиса.

### 8.2. error_rate

`error_rate` — метрика доли ошибок.

### 8.3. latency_p95

`latency_p95` — метрика 95-го перцентиля задержки.

### 8.4. Not applicable

`Not applicable` — метрика по смыслу не применяется к данному типу сервиса.

### 8.5. No data

`No data` — метрика должна существовать, но данных сейчас нет.

### 8.6. Unavailable

`Unavailable` — данные по метрике сейчас не удалось получить из-за деградации, ошибки или недоступности интеграции.

---

## 9. Инцидентный контекст и эксплуатационные действия

### 9.1. problem confirmation

`Problem confirmation` — прикладное событие подтверждения проблемной ситуации.

Оно:
- не создаёт отдельную incident entity;
- не меняет само по себе `current_status`;
- фиксируется в timeline и audit.

### 9.2. problem confirmation revoked

`Problem confirmation revoked` — прикладное событие отмены ранее подтверждённой проблемы.

### 9.3. overdue maintenance

`Overdue maintenance` — предупреждающий marker для случая, когда `planned_end_at` уже прошёл, но maintenance остаётся активным.

### 9.4. backend-only administrative flow

`Backend-only administrative flow` — контролируемый серверный сценарий административного изменения без публичного UI.

Допустимые формы V1:
- `controlled seed`
- `internal admin command`
- `migration-assisted assignment`
- эквивалентный controlled operational runbook

### 9.5. audit trail

`Audit trail` — прикладной след значимого действия, достаточный для проверки:
- кто выполнил действие;
- над чем выполнил;
- что изменил;
- когда выполнил;
- какой результат был достигнут.

---

## 10. Термины, которые не следует использовать как отдельные сущности V1

### 10.1. service notes

В V1 не вводится отдельная прикладная сущность `service_notes`.

Пользовательский operational context в V1 представляется через:
- `service_comments`
- `service_timeline_events`
- audit semantics, где это уместно

Термин `notes` не должен использоваться как название новой сущности, отдельной секции или нового data model object без явного обновления foundation.

### 10.2. manual override status

Под `manual override status` в V1 не понимается произвольная ручная установка прикладных статусов.

Единственный допустимый ручной статус V1 — `Maintenance`.

---

## 11. Consistency rule

Этот документ обязан быть синхронизирован с:
- `01_project_foundation.md`
- `05_role_permission_matrix.md`
- `08_data_model.md`
- `09_pages_and_navigation.md`
- `11_error_handling_and_degraded_mode.md`

Если возникает конфликт, `01_project_foundation.md` имеет приоритет.
