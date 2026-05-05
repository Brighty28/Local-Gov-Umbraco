# LocalGov Theme Source

Dev-only SCSS + Tailwind workspace. Compiles to
`../src/LocalGov.Umbraco.Theme/wwwroot/localgov/theme/localgov.css`,
which is committed and shipped inside the `LocalGov.Umbraco.Theme`
NuGet package. **Consumers of the package do not need Node.js** —
they get the compiled CSS via the meta-package.

## Stack

- **Tailwind CSS v3** — utility-first base. Preflight is disabled
  (we rely on GOV.UK Frontend's typography baseline).
- **Sass** — 7-1 architecture for hand-written component / layout styles.
- **PostCSS** — orchestrates Tailwind + autoprefixer + cssnano.
- **GOV.UK Frontend** — ships pre-compiled in the Theme RCL alongside
  this output, bundled together at runtime by WebOptimizer.

## One-time setup

```bash
cd theme-src
npm install
```

## Build

```bash
npm run build       # one-shot production build
npm run watch       # rebuilds on .scss / .css changes
```

Output goes to `../src/LocalGov.Umbraco.Theme/wwwroot/localgov/theme/localgov.css`.
Commit that file alongside your SCSS changes.

## Migrating from Foundation

1. Strip Foundation `@import` directives from your SCSS — Tailwind
   utilities replace the Foundation grid, button, and reveal patterns.
2. Move Foundation grid usage (`row` / `columns`) over to Tailwind
   (`grid grid-cols-12 gap-4` etc.) or the GOV.UK width container if
   the area should align with the rest of the GOV.UK frame.
3. Foundation mixins (e.g. `breakpoint`) translate to Tailwind's
   responsive prefixes (`md:`, `lg:`) or to plain Sass `@media`
   queries using the breakpoints in `scss/abstracts/_variables.scss`.

## 7-1 layout

```
scss/
├── abstracts/      — variables, mixins, functions (no CSS output)
├── base/           — typography, focus rings, resets
├── vendors/        — third-party CSS imported unchanged
├── layout/         — header, footer, grid, breadcrumb container
├── components/     — buttons, banners, nav cards, side nav
├── pages/          — page-specific overrides (use sparingly)
├── themes/         — council brand variants via CSS variables
└── main.scss       — entry point
```
