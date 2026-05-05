/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    '../src/**/Views/**/*.cshtml',
    '../src/**/wwwroot/**/*.html',
    './scss/**/*.scss',
    './main.css'
  ],
  theme: {
    extend: {
      colors: {
        // Driven by the lgSettings ContentType's primaryColour / secondaryColour fields.
        // Defaults match the GOV.UK Design System palette.
        'localgov-primary': 'var(--localgov-primary-colour, #1d70b8)',
        'localgov-secondary': 'var(--localgov-secondary-colour, #003078)'
      }
    }
  },
  // Avoid Tailwind's preflight reset wiping GOV.UK Frontend's typography baseline.
  // We rely on GOV.UK's own normalize/typography in govuk-frontend.min.css.
  corePlugins: {
    preflight: false
  },
  plugins: []
};
