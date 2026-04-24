# GOV.UK Frontend Assets

The `govuk-frontend.min.css` and `govuk-frontend.min.js` files are compiled from GOV.UK Frontend v5.x.

## Updating

```bash
npm install govuk-frontend@5
npx govuk-frontend-builder build
# copy dist files here
```

These files are committed to source control so consuming sites do not require Node.js at build time.
