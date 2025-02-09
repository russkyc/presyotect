import pluginJs from "@eslint/js";
import pluginVue from "eslint-plugin-vue";
import globals from "globals";
import tsEslint from "typescript-eslint";
import simpleImportSort from "eslint-plugin-simple-import-sort";

/** @type {import("eslint").Linter.Config[]} */
export default [
    {
        plugins: {
            "simple-import-sort": simpleImportSort
        }
    },
    {
        ignores: ["eslint.config.js", "node_modules", "dist", "dev-dist", "build", "coverage", "docs", "public", "static", "src/assets"],
    },
    {
        files: ["**/*.{js,mjs,cjs,ts,vue}"]
    },
    {
        languageOptions: {
            globals: {...globals.browser, ...globals.node}
        }
    },
    pluginJs.configs.recommended,
    ...tsEslint.configs.recommended,
    ...pluginVue.configs["flat/strongly-recommended"],
    {
        files: ["**/*.vue"],
        languageOptions: {
            parserOptions: {
                parser: tsEslint.parser
            }
        }
    },
    {
        rules: {
            "indent": ["error", 4],
            "prefer-const": "error",
            "vue/multi-word-component-names": "off",
            "simple-import-sort/imports": ["error", {
                // remove blank lines between import group
                "groups": [["^\\u0000", "^@?\\w", "^[^.]", "^\\."]],
            }],
            "simple-import-sort/exports": "error",
            quotes: ["error", "double"]
        },
    },
];