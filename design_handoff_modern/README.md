# Handoff: Finovo – „Modern" Design (Design 2)

## Overview

This package contains the high-fidelity design reference for a visual redesign of the **Finovo** Blazor web application — a B2B accounting & customer/project management tool targeting the Austrian market (German-language UI).

The selected design direction is **„Modern"**: a two-column split-pane layout with an icon sidebar that expands on demand, card-based content areas, and a contextual detail panel that opens alongside list views.

---

## About the Design Files

`Finovo Designs.html` is an **interactive HTML prototype** built with React + plain CSS. It is a **design reference**, not production code. Your task is to **recreate this design in the existing Blazor / MudBlazor codebase** (`Finovo.Web`) using the project's established patterns (Razor components, MudBlazor, `@rendermode InteractiveServer`).

Open the file in any browser. Select tab **„2. Modern"** (should already be active). Interact with the sidebar toggle, click customers/projects, use the search fields.

---

## Fidelity

**High-fidelity.** Colors, typography, spacing, border radii, shadow values, and interaction patterns are all specified exactly below and reflected in the prototype. Recreate pixel-accurately using MudBlazor components, overriding the MudTheme where necessary.

---

## CSS Architecture

**Do not use inline `style=` attributes in Razor components.** All styles must live in CSS files. Follow this separation:

### Rule
- **Shared / reusable styles** (cards, badges, avatars, progress bars, split-pane layout, etc.) → global CSS files in `wwwroot/Custom/`
- **Component-specific styles** (styles that only apply to one single `.razor` file) → scoped `.razor.css` file alongside the component
- **Design tokens** (CSS custom properties) + font imports → `wwwroot/app.css`

### File Structure

```
wwwroot/
  app.css                        ← CSS variables (tokens), font import, global resets
  Custom/
    cards.css                    ← .fn-card, .fn-card-header, .fn-kpi-card, .fn-section-title
    badges.css                   ← .fn-status-badge, .fn-chip, .fn-chip--active, .fn-chip--inactive
    avatars.css                  ← .fn-avatar, .fn-avatar--sm, .fn-avatar--lg, .fn-avatar--selected
    progress.css                 ← .fn-progress, .fn-progress__bar, .fn-progress__bar--danger
    split-pane.css               ← .fn-split, .fn-split__list, .fn-split__detail
    data-row.css                 ← .fn-data-row, .fn-data-row__label, .fn-data-row__value
    buttons.css                  ← .fn-btn, .fn-btn--primary, .fn-btn--outlined, .fn-btn--icon

Components/
  Layout/
    MainLayout.razor
    MainLayout.razor.css         ← sidebar collapse/expand, topbar, page shell (layout only)
    NavMenu.razor
    NavMenu.razor.css            ← nav item active states, section label, user row
  Pages/
    Home.razor
    Home.razor.css               ← dashboard-specific grid proportions, trend bar widths
    Customers/
      CustomerList.razor
      CustomerList.razor.css     ← list-pane row selected state, customer-specific detail header
    Projects/
      ProjectList.razor
      ProjectList.razor.css      ← project card grid, card selected state, side panel animation
  Shared/
    ProjectStatusChip.razor
    ProjectStatusChip.razor.css  ← status-specific color overrides (uses .fn-status-badge base)
```

### Loading Order

In `App.razor` (or `_Layout.cshtml`), load in this order so tokens are available before component styles:

```html
<link rel="stylesheet" href="app.css" />
<link rel="stylesheet" href="Custom/cards.css" />
<link rel="stylesheet" href="Custom/badges.css" />
<link rel="stylesheet" href="Custom/avatars.css" />
<link rel="stylesheet" href="Custom/progress.css" />
<link rel="stylesheet" href="Custom/split-pane.css" />
<link rel="stylesheet" href="Custom/data-row.css" />
<link rel="stylesheet" href="Custom/buttons.css" />
```

Scoped `.razor.css` files are loaded automatically by Blazor.

---

## Design Tokens — `wwwroot/app.css`

```css
@import url('https://fonts.googleapis.com/css2?family=DM+Sans:opsz,wght@9..40,300;9..40,400;9..40,500;9..40,600;9..40,700&family=Inter:wght@300;400;500;600;700&family=DM+Mono:wght@400;500&display=swap');

/* ── Finovo Design Tokens ── */
:root {
  /* Backgrounds */
  --fn-bg:              #f7f8fc;
  --fn-surface:         #ffffff;
  --fn-sidebar-bg:      #1c2540;
  --fn-sidebar-active:  #2d3d6b;

  /* Text */
  --fn-text:            #1c2033;
  --fn-text-sub:        #6c7491;
  --fn-sidebar-text:    #6b84c4;
  --fn-sidebar-text-on: #ffffff;

  /* Brand */
  --fn-primary:         #3b5bdb;
  --fn-primary-bg:      #eef1fd;
  --fn-accent:          #12b886;
  --fn-accent-bg:       #e6f9f0;
  --fn-danger:          #e05252;
  --fn-warning:         #f59f00;

  /* Structure */
  --fn-border:          #e8eaf0;
  --fn-shadow:          0 1px 4px rgba(60, 70, 120, 0.07);
  --fn-shadow-hover:    0 4px 12px rgba(60, 70, 120, 0.12);

  /* Radii */
  --fn-radius-card:     12px;
  --fn-radius-btn:      8px;
  --fn-radius-chip:     10px;
  --fn-radius-badge:    4px;
  --fn-radius-input:    8px;
  --fn-radius-avatar:   8px;
  --fn-radius-logo:     8px;

  /* Spacing */
  --fn-page-pad:        20px;
  --fn-card-pad:        16px 18px;
  --fn-gap:             12px;

  /* Typography */
  --fn-font-sans:       'Inter', sans-serif;
  --fn-font-display:    'DM Sans', sans-serif;
  --fn-font-mono:       'DM Mono', monospace;
}

html, body {
  font-family: var(--fn-font-sans);
  background: var(--fn-bg);
  color: var(--fn-text);
}
```

---

## Global CSS Files

### `wwwroot/Custom/cards.css`

Used by: Dashboard KPI cards, Dashboard list cards, Customer detail info cards, Project cards, any future panel.

```css
/* Base card */
.fn-card {
  background: var(--fn-surface);
  border-radius: var(--fn-radius-card);
  border: 1px solid var(--fn-border);
  box-shadow: var(--fn-shadow);
  overflow: hidden;
}

.fn-card:hover {
  box-shadow: var(--fn-shadow-hover);
}

/* Card with padding (info panels, detail sections) */
.fn-card--padded {
  padding: var(--fn-card-pad);
}

/* Card header row (title + action link) */
.fn-card-header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 14px 16px;
  border-bottom: 1px solid var(--fn-border);
}

.fn-card-header__title {
  font-size: 13px;
  font-weight: 600;
  color: var(--fn-text);
}

.fn-card-header__action {
  font-size: 11px;
  font-weight: 500;
  color: var(--fn-primary);
  background: none;
  border: none;
  cursor: pointer;
  text-decoration: none;
}

.fn-card-header__action:hover {
  text-decoration: underline;
}

/* Section title inside a card (e.g. "KONTAKT", "BUCHHALTUNG") */
.fn-section-title {
  font-size: 11px;
  font-weight: 600;
  color: var(--fn-text-sub);
  text-transform: uppercase;
  letter-spacing: 0.5px;
  margin-bottom: 12px;
}

/* KPI card (Dashboard stat cards) */
.fn-kpi-card {
  background: var(--fn-surface);
  border-radius: var(--fn-radius-card);
  border: 1px solid var(--fn-border);
  box-shadow: var(--fn-shadow);
  padding: 16px 18px;
}

.fn-kpi-card__label {
  font-size: 11px;
  font-weight: 500;
  color: var(--fn-text-sub);
}

.fn-kpi-card__value {
  font-family: var(--fn-font-display);
  font-size: 30px;
  font-weight: 700;
  color: var(--fn-text);
  letter-spacing: -1px;
  margin: 8px 0 4px;
}

.fn-kpi-card__sub {
  font-size: 10px;
  color: var(--fn-text-sub);
}

.fn-kpi-card__trend {
  font-size: 10px;
  font-weight: 700;
  border-radius: var(--fn-radius-badge);
  padding: 2px 6px;
}

/* Accent bar at bottom of KPI card */
.fn-kpi-card__bar {
  height: 2px;
  background: var(--fn-border);
  border-radius: 1px;
  margin-top: 12px;
  overflow: hidden;
}

.fn-kpi-card__bar-fill {
  height: 100%;
  width: 60%;
  border-radius: 1px;
}
```

### `wwwroot/Custom/badges.css`

Used by: ProjectStatusChip, CustomerList status column, Customer detail panel, Project cards.

```css
/* Status badge — pill style */
.fn-status-badge {
  display: inline-block;
  padding: 2px 10px;
  border-radius: var(--fn-radius-chip);
  font-size: 10px;
  font-weight: 600;
  letter-spacing: 0.2px;
  white-space: nowrap;
}

/* Status variants — colors set in ProjectStatusChip.razor.css */
.fn-status-badge--draft      { background: #2a2a3d; color: #9b9bc4; border: 1px solid rgba(155,155,196,0.19); }
.fn-status-badge--open       { background: #1d3a4a; color: #4db8d4; border: 1px solid rgba(77,184,212,0.19); }
.fn-status-badge--inprogress { background: #1a3d5c; color: #5ab4f5; border: 1px solid rgba(90,180,245,0.19); }
.fn-status-badge--onhold     { background: #3d2a1a; color: #f5a623; border: 1px solid rgba(245,166,35,0.19); }
.fn-status-badge--completed  { background: #1a3d2a; color: #5adb8e; border: 1px solid rgba(90,219,142,0.19); }
.fn-status-badge--cancelled  { background: #3d1a1a; color: #e05252; border: 1px solid rgba(224,82,82,0.19); }
.fn-status-badge--invoiced   { background: #1a3d2a; color: #2ecc71; border: 1px solid rgba(46,204,113,0.19); }

/* Inline chip (Aktiv / Inaktiv) */
.fn-chip {
  display: inline-block;
  padding: 2px 8px;
  border-radius: var(--fn-radius-chip);
  font-size: 10px;
  font-weight: 600;
  white-space: nowrap;
}

.fn-chip--active   { background: var(--fn-accent-bg);  color: var(--fn-accent); }
.fn-chip--inactive { background: var(--fn-bg);          color: var(--fn-text-sub); }

/* Square badge (e.g. project number, type label) */
.fn-badge {
  display: inline-block;
  padding: 2px 7px;
  border-radius: var(--fn-radius-badge);
  font-size: 10px;
  font-family: var(--fn-font-mono);
  color: var(--fn-text-sub);
  background: var(--fn-bg);
  border: 1px solid var(--fn-border);
}
```

### `wwwroot/Custom/avatars.css`

Used by: CustomerList rows, Customer detail header, Dashboard customer list, sidebar user row.

```css
.fn-avatar {
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: var(--fn-radius-avatar);
  font-weight: 700;
  font-family: var(--fn-font-sans);
  flex-shrink: 0;
  background: var(--fn-bg);
  color: var(--fn-text-sub);
  text-transform: uppercase;
  user-select: none;
}

/* Sizes */
.fn-avatar--sm  { width: 28px; height: 28px; font-size: 10px; }
.fn-avatar--md  { width: 34px; height: 34px; font-size: 12px; }
.fn-avatar--lg  { width: 52px; height: 52px; font-size: 18px; border-radius: 12px; }

/* Selected state (in customer list) */
.fn-avatar--selected {
  background: var(--fn-primary);
  color: #ffffff;
}

/* Circle avatar (sidebar user) */
.fn-avatar--circle {
  border-radius: 50%;
  width: 30px;
  height: 30px;
  font-size: 10px;
  background: var(--fn-sidebar-active);
  color: #8fa0c4;
}
```

### `wwwroot/Custom/progress.css`

Used by: Project cards budget bar, Project list budget column, Dashboard project list.

```css
.fn-progress {
  background: var(--fn-bg);
  border-radius: 5px;
  height: 6px;
  overflow: hidden;
}

/* Thinner variant for table rows */
.fn-progress--thin {
  height: 4px;
  border-radius: 3px;
}

.fn-progress__bar {
  height: 100%;
  border-radius: inherit;
  background: var(--fn-primary);
  transition: width 0.4s ease;
}

/* Danger threshold (>90%) */
.fn-progress__bar--danger {
  background: #e64980;
}
```

### `wwwroot/Custom/split-pane.css`

Used by: CustomerList (list + detail), ProjectList (cards + side panel).

```css
.fn-split {
  display: flex;
  height: 100%;
  overflow: hidden;
}

/* Left list pane */
.fn-split__list {
  width: 320px;
  flex-shrink: 0;
  border-right: 1px solid var(--fn-border);
  background: var(--fn-surface);
  display: flex;
  flex-direction: column;
  overflow: hidden;
}

.fn-split__list-body {
  flex: 1;
  overflow-y: auto;
}

/* Right detail pane */
.fn-split__detail {
  flex: 1;
  overflow: auto;
  padding: 24px;
  display: flex;
  flex-direction: column;
  gap: 16px;
}

/* Narrow side panel (Project cards) */
.fn-split__panel {
  width: 280px;
  flex-shrink: 0;
  border-left: 1px solid var(--fn-border);
  background: var(--fn-surface);
  overflow: auto;
  padding: 20px;
  transition: width 0.2s ease;
}

.fn-split__panel--hidden {
  width: 0;
  padding: 0;
  overflow: hidden;
}

/* List row */
.fn-list-row {
  padding: 12px 14px;
  border-bottom: 1px solid var(--fn-border);
  cursor: pointer;
  border-left: 3px solid transparent;
  transition: background 0.1s, border-color 0.1s;
}

.fn-list-row:hover {
  background: rgba(0, 0, 0, 0.02);
}

.fn-list-row--selected {
  background: rgba(59, 91, 219, 0.059);
  border-left-color: var(--fn-primary);
}
```

### `wwwroot/Custom/data-row.css`

Used by: Customer detail info cards, Project detail info cards, side panels.

```css
/* Label / value pair row */
.fn-data-row {
  display: flex;
  justify-content: space-between;
  align-items: baseline;
  padding: 8px 0;
  border-bottom: 1px solid var(--fn-border);
  gap: 12px;
}

.fn-data-row:last-child {
  border-bottom: none;
}

.fn-data-row__label {
  font-size: 12px;
  color: var(--fn-text-sub);
  flex-shrink: 0;
}

.fn-data-row__value {
  font-size: 12px;
  font-weight: 500;
  color: var(--fn-text);
  text-align: right;
}

/* Mono variant for IDs, numbers */
.fn-data-row__value--mono {
  font-family: var(--fn-font-mono);
  font-size: 11px;
}
```

### `wwwroot/Custom/buttons.css`

Used by: All action buttons throughout the app.

```css
.fn-btn {
  display: inline-flex;
  align-items: center;
  gap: 6px;
  padding: 7px 16px;
  border-radius: var(--fn-radius-btn);
  font-family: var(--fn-font-sans);
  font-size: 12px;
  font-weight: 600;
  cursor: pointer;
  border: none;
  transition: opacity 0.15s, box-shadow 0.15s;
  text-decoration: none;
  white-space: nowrap;
}

.fn-btn--primary {
  background: var(--fn-primary);
  color: #ffffff;
}

.fn-btn--primary:hover {
  opacity: 0.9;
  box-shadow: 0 2px 8px rgba(59, 91, 219, 0.3);
}

.fn-btn--outlined {
  background: var(--fn-surface);
  color: var(--fn-text);
  border: 1px solid var(--fn-border);
}

.fn-btn--outlined:hover {
  background: var(--fn-bg);
}

.fn-btn--icon {
  padding: 5px;
  width: 28px;
  height: 28px;
  justify-content: center;
  font-size: 13px;
  color: var(--fn-text-sub);
  background: transparent;
  border: 1px solid var(--fn-border);
}

.fn-btn--icon:hover {
  background: var(--fn-bg);
  color: var(--fn-text);
}

.fn-btn--icon-danger:hover {
  color: var(--fn-danger);
  border-color: var(--fn-danger);
}
```

---

## MudTheme Override — `MainLayout.razor`

```csharp
private readonly MudTheme _theme = new()
{
    PaletteLight = new PaletteLight
    {
        Primary          = "#3b5bdb",
        PrimaryDarken    = "#2f4ac7",
        PrimaryLighten   = "#5c7aff",
        Secondary        = "#12b886",
        AppbarBackground = "#ffffff",
        AppbarText       = "#1c2033",
        Background       = "#f7f8fc",
        Surface          = "#ffffff",
        DrawerBackground = "#1c2540",
        DrawerText       = "#6b84c4",
        DrawerIcon       = "#6b84c4",
        TextPrimary      = "#1c2033",
        TextSecondary    = "#6c7491",
        ActionDefault    = "#6c7491",
        Divider          = "#e8eaf0",
        TableLines       = "#e8eaf0",
        Success          = "#12b886",
        Warning          = "#f59f00",
        Error            = "#e05252",
        Info             = "#3b5bdb",
    },
    Typography = new Typography
    {
        Default = new DefaultTypography { FontFamily = ["Inter", "sans-serif"] },
        H5 = new H5Typography { FontFamily = ["DM Sans", "sans-serif"], FontWeight = "700" },
        H6 = new H6Typography { FontFamily = ["DM Sans", "sans-serif"], FontWeight = "700" }
    },
    LayoutProperties = new LayoutProperties
    {
        DrawerWidthLeft     = "200px",
        DrawerMiniWidthLeft = "64px",
        AppbarHeight        = "52px",
    }
};
```

---

## Scoped CSS Files

### `MainLayout.razor.css`

```css
/* Sidebar shell */
.sidebar {
  background: var(--fn-sidebar-bg);
  display: flex;
  flex-direction: column;
  overflow: hidden;
  transition: width 0.22s cubic-bezier(0.4, 0, 0.2, 1);
  flex-shrink: 0;
}

.sidebar--collapsed { width: 64px; }
.sidebar--expanded  { width: 200px; }

/* Logo header row */
.sidebar__logo-row {
  height: 56px;
  display: flex;
  align-items: center;
  padding: 0 12px;
  gap: 10px;
  border-bottom: 1px solid rgba(255, 255, 255, 0.07);
  flex-shrink: 0;
}

.sidebar__logo-placeholder {
  width: 30px;
  height: 30px;
  border-radius: var(--fn-radius-logo);
  background: linear-gradient(135deg, #2d3d6b 0%, #3b5bdb 100%);
  border: 1px solid rgba(255, 255, 255, 0.12);
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
  position: relative;
  overflow: hidden;
}

.sidebar__logo-placeholder-letter {
  font-family: var(--fn-font-display);
  font-size: 12px;
  font-weight: 800;
  color: #ffffff;
  position: relative;
  z-index: 1;
}

.sidebar__app-name {
  overflow: hidden;
  opacity: 0;
  transition: opacity 0.15s;
  white-space: nowrap;
}

.sidebar--expanded .sidebar__app-name { opacity: 1; }

.sidebar__app-name-primary {
  font-family: var(--fn-font-display);
  font-size: 14px;
  font-weight: 700;
  color: #ffffff;
  letter-spacing: -0.3px;
  line-height: 1.2;
  display: block;
}

.sidebar__app-name-sub {
  font-size: 9px;
  color: rgba(143, 160, 196, 0.8);
  text-transform: uppercase;
  letter-spacing: 0.4px;
  display: block;
}

/* Toggle button */
.sidebar__toggle {
  background: none;
  border: none;
  cursor: pointer;
  color: var(--fn-sidebar-text);
  font-size: 14px;
  padding: 4px;
  border-radius: 5px;
  line-height: 1;
  flex-shrink: 0;
}

.sidebar--expanded .sidebar__toggle { margin-left: auto; }

.sidebar__toggle:hover { background: rgba(255, 255, 255, 0.06); }

/* Top app bar */
.topbar {
  height: 52px;
  background: var(--fn-surface);
  border-bottom: 1px solid var(--fn-border);
  display: flex;
  align-items: center;
  padding: 0 20px;
  gap: 12px;
  flex-shrink: 0;
}

.topbar__title {
  font-family: var(--fn-font-display);
  font-size: 16px;
  font-weight: 700;
  color: var(--fn-text);
  flex: 1;
}

.topbar__search {
  display: flex;
  align-items: center;
  gap: 8px;
  background: var(--fn-bg);
  border: 1px solid var(--fn-border);
  border-radius: var(--fn-radius-input);
  padding: 6px 12px;
  font-size: 12px;
  color: var(--fn-text-sub);
  cursor: pointer;
}

.topbar__search-kbd {
  font-family: var(--fn-font-mono);
  font-size: 10px;
  background: var(--fn-border);
  border-radius: 3px;
  padding: 1px 5px;
  margin-left: 6px;
}
```

### `NavMenu.razor.css`

```css
.nav-section-label {
  font-size: 9px;
  font-weight: 600;
  color: #4a5a80;
  text-transform: uppercase;
  letter-spacing: 0.9px;
  padding: 6px 8px 4px;
}

.nav-item {
  display: flex;
  align-items: center;
  width: 100%;
  height: 40px;
  border-radius: 8px;
  border: none;
  cursor: pointer;
  background: transparent;
  color: var(--fn-sidebar-text);
  font-family: var(--fn-font-sans);
  transition: all 0.15s;
  overflow: hidden;
  white-space: nowrap;
}

.nav-item--collapsed {
  justify-content: center;
  padding: 0;
  gap: 0;
}

.nav-item--expanded {
  justify-content: flex-start;
  padding: 9px 10px;
  gap: 10px;
}

.nav-item--active {
  background: var(--fn-sidebar-active);
  color: #ffffff;
}

.nav-item:not(.nav-item--active):hover {
  background: rgba(255, 255, 255, 0.05);
}

.nav-item__label {
  font-size: 13px;
  font-weight: 400;
}

.nav-item--active .nav-item__label {
  font-weight: 600;
}
```

### `CustomerList.razor.css`

```css
/* Search bar in list pane */
.customer-list__search {
  padding: 12px 14px;
  border-bottom: 1px solid var(--fn-border);
}

.customer-list__search-input {
  display: flex;
  align-items: center;
  gap: 8px;
  background: var(--fn-bg);
  border: 1px solid var(--fn-border);
  border-radius: var(--fn-radius-input);
  padding: 7px 10px;
  font-size: 12px;
  color: var(--fn-text);
  width: 100%;
}

/* Detail header */
.customer-detail__header {
  display: flex;
  align-items: flex-start;
  justify-content: space-between;
  gap: 14px;
}

.customer-detail__name {
  font-family: var(--fn-font-display);
  font-size: 18px;
  font-weight: 700;
  color: var(--fn-text);
}

.customer-detail__sub {
  font-size: 12px;
  color: var(--fn-text-sub);
  margin-top: 2px;
}
```

### `ProjectList.razor.css`

```css
/* Card grid area */
.project-grid {
  flex: 1;
  overflow: auto;
  padding: 16px;
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(280px, 1fr));
  align-content: start;
  gap: var(--fn-gap);
}

/* Individual project card */
.project-card {
  background: var(--fn-surface);
  border-radius: var(--fn-radius-card);
  border: 2px solid var(--fn-border);
  padding: 16px;
  cursor: pointer;
  transition: border-color 0.15s, box-shadow 0.15s;
  box-shadow: var(--fn-shadow);
}

.project-card:hover {
  box-shadow: var(--fn-shadow-hover);
}

.project-card--selected {
  border-color: var(--fn-primary);
  box-shadow: 0 0 0 3px rgba(59, 91, 219, 0.125);
}

.project-card__top {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  margin-bottom: 10px;
}

.project-card__name {
  font-family: var(--fn-font-display);
  font-size: 14px;
  font-weight: 700;
  color: var(--fn-text);
  margin-bottom: 4px;
}

.project-card__customer {
  font-size: 11px;
  color: var(--fn-text-sub);
  margin-bottom: 12px;
}

.project-card__budget-row {
  display: flex;
  justify-content: space-between;
  margin-top: 4px;
  font-size: 10px;
  color: var(--fn-text-sub);
}

.project-card__budget-mono {
  font-family: var(--fn-font-mono);
}

.project-card__footer {
  display: flex;
  justify-content: space-between;
  margin-top: 10px;
  padding-top: 10px;
  border-top: 1px solid var(--fn-border);
  font-size: 10px;
  color: var(--fn-text-sub);
}

.project-card__invoiced {
  font-weight: 600;
  color: var(--fn-accent);
}
```

### `ProjectStatusChip.razor.css`

```css
/* Uses .fn-status-badge base from badges.css */
/* Only override if extra component-level tweaks are needed */
```

### `Home.razor.css`

```css
.dashboard {
  overflow: auto;
  height: 100%;
  padding: var(--fn-page-pad);
  display: flex;
  flex-direction: column;
  gap: 16px;
}

.dashboard__kpi-grid {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: var(--fn-gap);
}

.dashboard__content-grid {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: var(--fn-gap);
}
```

---

## Updated `ProjectStatusChip.razor`

Use CSS classes from `badges.css` instead of inline styles or `Color.*`:

```razor
@using Finovo.Domain.Enums

<span class="fn-status-badge @GetCssClass(Status)">@GetLabel(Status)</span>

@code {
    [Parameter] public ProjectStatus Status { get; set; }

    private static string GetCssClass(ProjectStatus status) => status switch
    {
        ProjectStatus.Draft      => "fn-status-badge--draft",
        ProjectStatus.Open       => "fn-status-badge--open",
        ProjectStatus.InProgress => "fn-status-badge--inprogress",
        ProjectStatus.OnHold     => "fn-status-badge--onhold",
        ProjectStatus.Completed  => "fn-status-badge--completed",
        ProjectStatus.Cancelled  => "fn-status-badge--cancelled",
        ProjectStatus.Invoiced   => "fn-status-badge--invoiced",
        _                        => "fn-status-badge--draft",
    };

    private static string GetLabel(ProjectStatus status) => status switch
    {
        ProjectStatus.Draft      => "Entwurf",
        ProjectStatus.Open       => "Offen",
        ProjectStatus.InProgress => "In Arbeit",
        ProjectStatus.OnHold     => "Pausiert",
        ProjectStatus.Completed  => "Abgeschlossen",
        ProjectStatus.Cancelled  => "Storniert",
        ProjectStatus.Invoiced   => "Verrechnet",
        _                        => status.ToString()
    };
}
```

---

## Screens / Views

### 1. Overall Shell — `MainLayout.razor`

**Layout:** Full viewport `display:flex; flex-direction:row`. Sidebar (`.sidebar`) on the left, main content area (flex:1) on the right containing topbar + page content.

#### Sidebar — collapsed (64px)

- CSS class: `.sidebar.sidebar--collapsed`
- Logo row: `.sidebar__logo-row` — logo placeholder + hidden app name + expand toggle (`›`)
- Nav items: `.nav-item.nav-item--collapsed` — icon only, centered, MudTooltip for label on hover
- User avatar at bottom: `.fn-avatar.fn-avatar--circle`

#### Sidebar — expanded (200px)

- CSS class: `.sidebar.sidebar--expanded`
- Logo row: placeholder + `.sidebar__app-name` becomes visible (opacity: 1) + collapse toggle (`‹`) at margin-left: auto
- Section label: `.nav-section-label` — „VERWALTUNG"
- Nav items: `.nav-item.nav-item--expanded` — icon + label side by side
- User avatar row: avatar + name + role visible

#### Top App Bar

- Class: `.topbar`
- Title: `.topbar__title`
- Search: `.topbar__search` + `.topbar__search-kbd`
- Action button: `.fn-btn.fn-btn--primary`

---

### 2. Dashboard — `/` (`Home.razor`)

- Outer: `.dashboard`
- KPI row: `.dashboard__kpi-grid` containing 4× `.fn-kpi-card`
- Content row: `.dashboard__content-grid` containing 2× `.fn-card`
  - Each card header: `.fn-card-header` with `.fn-card-header__title` + `.fn-card-header__action`
  - Customer rows in card: `.fn-list-row` (from split-pane.css) with `.fn-avatar.fn-avatar--sm` + `.fn-chip.fn-chip--active/--inactive`
  - Project rows in card: `.fn-data-row` for budget info + `.fn-progress.fn-progress--thin` + `.fn-status-badge`

---

### 3. Customer List — `/kunden` (`CustomerList.razor`)

- Outer: `.fn-split`
- Left: `.fn-split__list`
  - Search: `.customer-list__search` + `.customer-list__search-input` (use MudTextField inside)
  - Rows: `.fn-list-row` / `.fn-list-row.fn-list-row--selected`
    - Avatar: `.fn-avatar.fn-avatar--md` / `.fn-avatar--selected`
    - Status: `.fn-chip.fn-chip--active` or `.fn-chip--inactive`
- Right: `.fn-split__detail`
  - Header: `.customer-detail__header` with `.fn-avatar.fn-avatar--lg.fn-avatar--selected`
  - Info grid: 2-column CSS grid, each cell `.fn-card.fn-card--padded`
    - Section title: `.fn-section-title`
    - Rows: `.fn-data-row` with `.fn-data-row__label` + `.fn-data-row__value`
  - Projects card: `.fn-card.fn-card--padded` with `.fn-card-header` + project rows using `.fn-data-row` + `.fn-status-badge`
  - Action buttons: `.fn-btn.fn-btn--outlined` + `.fn-btn.fn-btn--primary`

---

### 4. Project List — `/projekte` (`ProjectList.razor`)

- Outer: `.fn-split`
- Left: `.project-grid` (replaces `.fn-split__list`)
- Each card: `.project-card` / `.project-card.project-card--selected`
  - Top: `.project-card__top` — `.fn-badge` (project number) + `.fn-status-badge`
  - Name: `.project-card__name`
  - Customer: `.project-card__customer`
  - Budget bar: `.fn-progress` + `.fn-progress__bar` / `.fn-progress__bar--danger`
  - Budget row: `.project-card__budget-row` + `.project-card__budget-mono`
  - Footer: `.project-card__footer` + `.project-card__invoiced`
- Right panel: `.fn-split__panel` / `.fn-split__panel--hidden`
  - Rows: `.fn-data-row` + `.fn-data-row__label` + `.fn-data-row__value`
  - Buttons: `.fn-btn.fn-btn--primary` + `.fn-btn.fn-btn--outlined`

---

## Interactions & Behavior

### Sidebar Toggle
- State: `bool _sideExpanded = false` in `MainLayout.razor`
- Toggle adds/removes `.sidebar--collapsed` / `.sidebar--expanded` CSS classes
- CSS transition `width 0.22s cubic-bezier(0.4,0,0.2,1)` handles animation

### Customer Split-Pane
- State: `CustomerDto? _selectedCustomer` in `CustomerList.razor`
- Row click: `_selectedCustomer = customer` (no navigation)
- Row CSS: adds `.fn-list-row--selected` + `.fn-avatar--selected` when selected
- `@onclick:stopPropagation` on action buttons inside rows

### Project Card Selection
- State: `ProjectDto? _selectedProject` in `ProjectList.razor`
- Card click: toggle `_selectedProject`; same card or `✕` clears
- Card CSS: `.project-card--selected` when active
- Panel CSS: `.fn-split__panel` / `.fn-split__panel--hidden` drives slide animation

### Budget Progress
- Compute `bool overBudget = pct > 90` in code-behind
- Apply `.fn-progress__bar--danger` class when `overBudget` is true

### Hover States
- All handled via CSS `:hover` pseudo-class in the relevant files above

---

## State Management

| Component | New state | Notes |
|---|---|---|
| `MainLayout.razor` | `bool _sideExpanded` | Optionally persist in `localStorage` via JS interop |
| `CustomerList.razor` | `CustomerDto? _selectedCustomer` | Replaces navigation to `/kunden/{id}` from list |
| `ProjectList.razor` | `ProjectDto? _selectedProject` | New — was previously navigate-only |

No new services or DTOs required.

---

## Files to Create / Modify

| File | Action | Notes |
|---|---|---|
| `wwwroot/app.css` | **Modify** | Add font import + CSS token variables |
| `wwwroot/Custom/cards.css` | **Create** | Shared card styles |
| `wwwroot/Custom/badges.css` | **Create** | Status badges, chips |
| `wwwroot/Custom/avatars.css` | **Create** | Avatar sizes + variants |
| `wwwroot/Custom/progress.css` | **Create** | Budget progress bar |
| `wwwroot/Custom/split-pane.css` | **Create** | Two-pane layout shell |
| `wwwroot/Custom/data-row.css` | **Create** | Label/value row pairs |
| `wwwroot/Custom/buttons.css` | **Create** | Button variants |
| `App.razor` (or `_Layout.cshtml`) | **Modify** | Add `<link>` tags for all Custom/*.css files |
| `MainLayout.razor` | **Modify** | New MudTheme, `_sideExpanded` state |
| `MainLayout.razor.css` | **Modify** | Sidebar + topbar scoped styles |
| `NavMenu.razor` | **Modify** | Accept `_sideExpanded` parameter, apply nav CSS classes |
| `NavMenu.razor.css` | **Modify** | Nav item scoped styles |
| `Home.razor` | **Modify** | Apply `.fn-kpi-card`, `.fn-card`, new grid classes |
| `Home.razor.css` | **Modify** | Dashboard grid layout |
| `CustomerList.razor` | **Modify** | Replace table with split-pane layout |
| `CustomerList.razor.css` | **Modify** | Customer-specific scoped styles |
| `ProjectList.razor` | **Modify** | Replace table with card grid + side panel |
| `ProjectList.razor.css` | **Modify** | Project card + grid scoped styles |
| `ProjectStatusChip.razor` | **Modify** | Use CSS class approach (see above) |
| `ProjectStatusChip.razor.css` | **Keep/empty** | Base styles now in badges.css |

---

## Implementation Order (for Claude Code)

1. **Tokens + fonts** — `app.css` variables + font import. Load in `App.razor`.
2. **Global CSS files** — Create all `wwwroot/Custom/*.css` files. Link them in `App.razor`.
3. **MudTheme** — Update `MainLayout.razor`. Verify existing pages adopt new colors.
4. **ProjectStatusChip** — Switch to CSS class approach. Check all pages.
5. **Sidebar** — `MainLayout.razor.css` + `NavMenu.razor.css` + toggle state. Test animation.
6. **Dashboard** — `Home.razor` + `Home.razor.css` — apply KPI card + list card classes.
7. **Project List** — `ProjectList.razor` card grid + side panel.
8. **Customer List** — `CustomerList.razor` split-pane + inline detail.

---

## Assets

- **Logo:** Replace `.sidebar__logo-placeholder` with `<img src="/logo.svg" class="sidebar__logo-img" />`. Place logo at `wwwroot/logo.svg`.
- **Fonts:** DM Sans, Inter, DM Mono via Google Fonts. For production, self-host in `wwwroot/fonts/` and replace the `@import` with local `@font-face` declarations.
- **Icons:** Use existing `@Icons.Material.Filled.*` from MudBlazor throughout.

---

## Reference File

`Finovo Designs.html` — open in any modern browser, select tab **„2. Modern"**. Fully interactive — click the sidebar toggle, navigate pages, select customers and projects.
