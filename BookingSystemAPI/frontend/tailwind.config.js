/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ['./src/**/*.{html,ts,scss}'],
  // Importante: usar 'class' para evitar conflictos con Angular Material
  darkMode: 'class',
  theme: {
    extend: {
      colors: {
        // Colores personalizados que complementan Angular Material
        primary: {
          50: '#f5e6ff',
          100: '#e5ccff',
          200: '#cc99ff',
          300: '#b266ff',
          400: '#9933ff',
          500: '#8000ff',
          600: '#6600cc',
          700: '#4d0099',
          800: '#330066',
          900: '#1a0033',
        },
        secondary: {
          50: '#fff5e6',
          100: '#ffe5cc',
          200: '#ffcc99',
          300: '#ffb266',
          400: '#ff9933',
          500: '#ff8000',
          600: '#cc6600',
          700: '#994d00',
          800: '#663300',
          900: '#331a00',
        },
      },
      spacing: {
        // Espaciados adicionales útiles
        '18': '4.5rem',
        '22': '5.5rem',
        '30': '7.5rem',
      },
      fontFamily: {
        sans: ['Roboto', 'Helvetica Neue', 'sans-serif'],
      },
    },
  },
  // Deshabilitar estilos base que pueden conflictuar con Material
  corePlugins: {
    preflight: false,
  },
  plugins: [],
};
