# Verdant theme

Forest-green / nature aesthetic inspired by modern UK council sites that have
moved past the GOV.UK Frontend default palette. Pairs a navy logo block with
a white pill-nav bar, deep-green primary actions, sage-cream surfaces, and
rounded shapes throughout.

## Activate

In the consumer app's `appsettings.json`:

```json
{
  "LocalGov": {
    "Theme": "verdant"
  }
}
```

WebOptimizer reads this on startup and bundles
`/_content/LocalGov.Umbraco.Theme/localgov/theme/verdant.css` instead of the
default. The bundled URL stays `/css/localgov.css` either way, so consumer
layouts don't need to change.

## Build

From `theme-src/`:

```bash
npm run build:verdant       # one-shot
npm run watch:verdant       # rebuilds on .scss / .css changes
npm run build               # builds every theme
```

Output: `../../src/LocalGov.Umbraco.Theme/wwwroot/localgov/theme/verdant.css`.
Commit the regenerated CSS alongside SCSS edits.

## Layout-specific markup

The verdant CSS includes hooks for two patterns the default theme doesn't have.
Use them in your views to get the full South-Cambs-style look:

### Photo hero with floating card

```html
<section class="localgov-hero localgov-hero--photo"
         style="background-image: url('@heroImage.Url()')">
    <div class="localgov-hero__inner">
        <div class="localgov-hero__card">
            <h2 class="localgov-hero__card-title">In my area</h2>
            <p class="localgov-hero__card-body">
                Enter your postcode to see information for your area...
            </p>
            <form class="localgov-postcode" action="/in-my-area" method="get">
                <input class="localgov-postcode__input" name="postcode"
                       type="text" placeholder="e.g. CB23 6EA" />
                <button class="localgov-postcode__button" type="submit">
                    Go
                </button>
            </form>
        </div>
    </div>
</section>
```

### 4-column action bar

```html
<div class="localgov-action-bar">
    <a class="localgov-action-bar__item" href="/book">Book</a>
    <a class="localgov-action-bar__item" href="/pay">Pay</a>
    <a class="localgov-action-bar__item" href="/notify">Notify</a>
    <a class="localgov-action-bar__item" href="/apply">Apply</a>
</div>
```

Drop these blocks into `lgHome.cshtml` (or wherever you want them) and the
verdant CSS will style them. The default theme ignores the new class names
gracefully — they simply render unstyled if you switch back.
