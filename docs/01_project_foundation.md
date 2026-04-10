# 01_project_foundation.md

## 1. Назначение документа

Этот документ является foundation-документом проекта **Manage MegaPayment Service**.

Он фиксирует:
- назначение системы;
- границы `MVP V1`, `Full V1` и `Out of Scope`;
- ключевые роли;
- ключевые сущности и инварианты предметной области;
- высокоуровневую статусную модель;
- высокоуровневые правила доступа;
- главные архитектурные решения;
- критерии готовности V1 на уровне продукта.

Этот документ является **главным источником истины для product boundaries, business invariants и high-level architecture decisions**.

## 2. Что не должно жить в foundation

В этом документе не должны канонически храниться:
- exact API contracts;
- field-level payload shapes;
- detailed permission matrix;
- formal status precedence и transition tables;
- page-level UI behavior;
- schema-level DB details;
- exact numeric thresholds;
- runbook execution procedures;
- literal startup commands и environment-specific configuration.

Эти темы фиксируются в дочерних документах.

## 3. Общее описание системы

**Manage MegaPayment Service** — внутренняя банковская веб-система для наблюдения за состоянием платёжных сервисов и работы с эксплуатационным контекстом вокруг них.

Система предназначена для:
- просмотра каталога сервисов;
- отображения текущего состояния сервисов;
- просмотра истории смены состояний;
- хранения и просмотра служебных комментариев;
- хранения и просмотра service links;
- просмотра базовой динамики технических метрик;
- предоставления единой операционной панели для сопровождения сервисной инфраструктуры.

Система:
- не выполняет платёжные операции;
- не является клиентским банковским продуктом;
- не заменяет стек мониторинга;
- не является полноценной incident-management платформой.

## 4. Проблема и цель

### 4.1. Проблема

Информация о состоянии сервисов разрознена между мониторингом, таблицами, runbook-материалами, отдельными диагностическими системами и локальными знаниями сотрудников.

### 4.2. Цель

Цель проекта — дать единую внутреннюю систему, которая объединяет:
- каталог сервисов;
- текущее состояние;
- историю статусов;
- комментарии;
- service links;
- базовые метрики;
- прикладной эксплуатационный контекст.

## 5. Границы проекта

### 5.1. MVP V1

В `MVP V1` входят:
- главная страница со сводкой;
- каталог сервисов;
- карточка сервиса;
- категории;
- текущий статус сервиса;
- история статусов;
- служебные комментарии;
- service links;
- service timeline;
- базовые метрики из Prometheus;
- роли доступа;
- хранение предметных данных в PostgreSQL;
- авторитетный источник текущего состояния сервиса;
- сопоставление service-to-status-source;
- сопоставление service-to-metrics.

### 5.2. Full V1

В `Full V1` дополнительно могут входить:
- базовые экраны пользователей и ролей;
- минимальный административный раздел;
- admin/data-quality view;
- расширенное представление service links;
- интервал метрик `30d`, если retention в Prometheus это позволяет;
- базовая синхронизация пользователей с внешним источником.

### 5.3. Out of Scope

В V1 не входят:
- полноценный модуль управления инцидентами;
- workflow согласования;
- автоматическая оркестрация инцидентов;
- автоматическое восстановление сервисов;
- хранение полного потока метрик в PostgreSQL;
- тяжёлая аналитика;
- граф зависимостей между сервисами;
- мобильный интерфейс.

## 6. Роли V1

В системе существуют только следующие прикладные роли:
- `Operator`
- `TechnicalSpecialist`
- `ServiceOwner`
- `Administrator`

Набор ролей V1 является фиксированным. Создание новых прикладных ролей через обычные административные use cases не поддерживается.

### 6.1. Смысл ролей

- `Operator` — read-only наблюдение за состоянием сервисов.
- `TechnicalSpecialist` — работа с эксплуатационным контекстом и problem confirmation.
- `ServiceOwner` — поддержка данных и service links в рамках собственных сервисов.
- `Administrator` — управление сервисами, категориями, ролями, владельцами и административными сценариями.

Детальная permission-matrix фиксируется в `05_role_permission_matrix.md`.

## 7. Ключевые сущности и инварианты

Основные сущности V1:
- `Service`
- `Category`
- `User`
- `Role`
- `ServiceOwnerRelation`
- `ServiceComment`
- `StatusHistory`
- `ServiceTimelineEvent`
- `ServiceLink`
- `IntegrationSyncState`

Ключевые инварианты:
- `Service` — центральная сущность системы.
- Для active service обязательны owner, валидная category, integration keys и обязательные metadata.
- `ServiceOwner` как прикладная роль и ownership relation — не одно и то же.
- Owner-scoped права требуют одновременно роли `ServiceOwner` и активной ownership relation.
- Comments в V1 — append-only.
- `status_history` — историческая immutable-запись.
- Comments и timeline — разные сущности.
- Problem confirmation не создаёт отдельную incident entity.

Schema-level детализация фиксируется в `08_data_model.md`.

## 8. Статусная модель V1

Для V1 используются следующие прикладные статусы:
- `OK`
- `Degraded`
- `Down`
- `Unknown`
- `Maintenance`

Высокоуровневые правила:
- статус сервиса — это нормализованное прикладное состояние, а не сырая метрика;
- `OK`, `Degraded`, `Down`, `Unknown` не устанавливаются вручную обычными пользователями;
- `Maintenance` — единственный допустимый ручной статус;
- `Maintenance` имеет приоритет как текущий прикладной статус;
- при недоверенном состоянии источника или устаревании данных сервис может переходить в `Unknown`.

Формальная status model фиксируется в `06_status_model.md`.

## 9. Правила доступа высокого уровня

- Разграничение доступа должно применяться в UI, backend authorization и application use cases.
- UI-only hiding не считается достаточным enforcement.
- Пользователь без локальной прикладной роли не получает доступ к системе.
- Административные действия должны быть audit-friendly.
- Owner-scoped access требует одновременно роли `ServiceOwner` и ownership relation.

Детальная permission-matrix — в `05_role_permission_matrix.md`.

## 10. Identity & Access высокого уровня

- Аутентификация V1 выполняется через внешний корпоративный OIDC-механизм.
- После успешной аутентификации система работает с локальной прикладной записью `User`.
- Локальные роли являются источником истины внутри приложения.
- Для V1 должен существовать bootstrap-механизм назначения первого администратора.
- Backend-only administrative flows допустимы только как controlled operational path.

Подробности фиксируются в:
- `02_solution_design.md`
- `12_run_guide.md`
- `14_environment_and_run.md`

## 11. Интеграции и архитектура высокого уровня

### 11.1. Интеграции

- PostgreSQL хранит предметные данные.
- Prometheus используется как источник временных рядов и динамики метрик.
- Авторитетный источник состояния обязателен для `current_status`.
- Приложение не должно дублировать весь observability stack внутри прикладной БД.

### 11.2. Архитектура

Для V1 фиксируется:
- **Blazor Server**
- **ASP.NET Core**
- **PostgreSQL**
- **Prometheus**

Проект должен быть разделён на слои:
- `Domain`
- `Application`
- `Infrastructure`
- `Web`

Высокоуровневые архитектурные правила:
- UI не определяет `current_status`;
- нормализация статуса выполняется server-side логикой;
- бизнес-валидация activation и административных операций живёт в `Application`;
- внешние интеграции не должны напрямую утекать в UI.

## 12. Критерии готовности V1 высокого уровня

V1 считается готовой на уровне продукта, если:
- каталог работает как единый прикладной источник обзора сервисов;
- главная страница даёт быстрый operational overview;
- карточка сервиса даёт достаточный первичный контекст;
- история статусов, comments, timeline и service links реально работают;
- роли ограничивают действия не только визуально, но и серверно;
- `Unknown` и `Maintenance` реализованы единообразно;
- частичная деградация одной интеграции не ломает всё приложение;
- bootstrap первого администратора и auditability значимых административных действий определены.

## 13. Декомпозиция на дочерние документы

- `02_solution_design.md` — contracts, API/payload shapes, integration flows.
- `03_implementation_plan.md` — staged delivery plan.
- `04_glossary.md` — каноническая терминология.
- `05_role_permission_matrix.md` — детальная permission-matrix.
- `06_status_model.md` — formal status rules, precedence, transitions.
- `07_prometheus_metrics_dynamics.md` — metrics applicability, ranges, query policy.
- `08_data_model.md` — schema-level model, constraints, invariants.
- `09_pages_and_navigation.md` — page behavior, markers, section states.
- `10_non_functional_requirements.md` — exact numeric thresholds и NFR baseline.
- `11_error_handling_and_degraded_mode.md` — degraded/error semantics.
- `12_run_guide.md` — backend-only admin runbooks.
- `13_decision_log.md` — ADR / decision record.
- `14_environment_and_run.md` — setup/process-level run rules и startup verification.

## 14. Правило приоритета

Если дочерний документ конфликтует с foundation:
1. источником истины остаётся `01_project_foundation.md`;
2. exact numeric thresholds берутся из `10_non_functional_requirements.md`;
3. status formalization — из `06_status_model.md`;
4. detailed permissions — из `05_role_permission_matrix.md`;
5. schema-level storage decisions — из `08_data_model.md`;
6. literal operational/admin procedures — из `12_run_guide.md`;
7. setup / startup process — из `14_environment_and_run.md`.
