import tailwindCssPrimeUi from "tailwindcss-primeui";

/** @type {import('tailwindcss').Config} */
export default {
    darkMode: "class",
    content: [
        "./index.html",
        "./src/**/*.{vue,js,ts,jsx,tsx}",
    ],
    theme: {
        extend: {
            transitionProperty: {
                width: "width",
            },
        }
    },
    plugins: [tailwindCssPrimeUi],
}

