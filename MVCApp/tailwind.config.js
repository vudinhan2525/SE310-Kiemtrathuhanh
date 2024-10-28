/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./Views/**/*.cshtml", // Include Razor views
    "./wwwroot/**/*.html", // Include static HTML files if any
  ],
  theme: {
    extend: {},
  },
  plugins: [],
};
