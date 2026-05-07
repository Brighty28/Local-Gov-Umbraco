# LocalGov Theme Source

Dev-only SCSS + Tailwind workspace. Compiles each theme to a stylesheet under
`../src/LocalGov.Umbraco.Theme/wwwroot/localgov/theme/`. The compiled CSS is
committed and shipped inside the `LocalGov.Umbraco.Theme` NuGet package, so
**consumers do not need Node.js**.

## Why isn't this folder showing in Visual Studio?

`theme-src/` is a Node.js workspace, not a .NET project, and the
`.slnx` solution format only tracks `.csproj` projects. Open it
separately:

- **Recommended**: open the folder in VS Code (`code theme-src`).
- Alternative: in Visual Studio, *File → Open → Folder...* and pick the
  `theme-src/` directory.

## Themes shipped

Two themes are included; pick one via `appsettings.json` in the consumer site:

| Theme name  | Aesthetic                                      | Output               |
| ----------- | ---------------------------------------------- | -------------------- |
| `default`   | GOV.UK Frontend modernised — blue gradient,    | `localgov.css`       |
|             | rounded cards, subtle shadows.                 |                      |
| `verdant`   | Forest-green / nature — navy logo block,       | `verdant.css`        |
|             | white pill nav, sage-cream cards, deep         |                      |
|             | green primary, photo hero with floating card.  |                      |

```json
{
  "LocalGov": { "Theme": "verdant" }
}
```

The WebOptimizer pipeline in `LocalGov.Umbraco.Core` reads this on startup and
bundles the matching CSS file into `/css/localgov.css`. Setting `default` (or
omitting the value) selects `localgov.css`.

## Stack

- **Tailwind CSS v3** — utility-first base. Preflight is disabled (we rely on
  GOV.UK Frontend's typography baseline).
- **Sass** — 7-1 architecture for hand-written component / layout styles.
- **PostCSS** — orchestrates Tailwind + autoprefixer + cssnano.
- **GOV.UK Frontend** — ships pre-compiled in the Theme RCL alongside this
  output, bundled together at runtime by WebOptimizer.

## One-time setup

```bash
cd theme-src
npm install
```

## Build

```bash
npm run build           # builds every theme
npm run build:default   # builds just the default theme
npm run build:verdant   # builds just the verdant theme
npm run watch           # rebuilds every theme on .scss / .css changes
```

**Commit the regenerated CSS files alongside SCSS edits.** The repo intentionally
tracks both source and compiled output so consumer builds never need Node.

## Adding a new theme

1. Create `themes/{name}/main.css` and `themes/{name}/scss/main.scss` (copy
   one of the existing themes as a starting point).
2. Add `build:{name}:scss`, `build:{name}:css`, `build:{name}`, and
   `watch:{name}` scripts to `package.json` mirroring the verdant ones.
3. Add `build:{name}` to the top-level `build` script.
4. Run `npm run build:{name}` to produce
   `../src/LocalGov.Umbraco.Theme/wwwroot/localgov/theme/{name}.css`.
5. Commit the new CSS file.
6. Set `"LocalGov": { "Theme": "{name}" }` in your consumer site's
   `appsettings.json` to activate it.

## How council brand colours flow through

1. Editor sets `primaryColour` (and optionally `secondaryColour`) on the
   `lgSettings` ContentType in the back office.
2. `ThemeTagHelper` (in `LocalGov.Umbraco.Theme`) emits an inline `<style>` on
   every page setting `--localgov-primary-colour` etc. on `:root`.
3. All component CSS in this workspace references those custom properties —
   not the SCSS `$colour-primary` variable directly — so brand changes apply
   instantly without rebuilding CSS, and they work across every theme.
