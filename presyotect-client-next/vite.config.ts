import {fileURLToPath, URL} from "url";
import {defineConfig} from 'vite'
import {VitePWA} from 'vite-plugin-pwa'
import vue from '@vitejs/plugin-vue'

// https://vite.dev/config/
export default defineConfig({
    plugins: [
        vue(),
        VitePWA({
            registerType: 'autoUpdate',
            workbox: {
                globPatterns: ['**/*.{js,css,html,ico,png,svg,jpg,jpeg}'],
                navigateFallbackDenylist: [/^\/_/]
            },
            injectRegister: 'script',
            devOptions: {
                enabled: true,
                type: 'module'
            },
            manifest: {
                name: 'Presyotect VNext',
                short_name: 'Presyotect Next',
                description: 'Presyotect Price Monitoring System Preview Version',
                theme_color: '#192f70',
                icons: [
                    {
                        src: './pwa/64x64.png',
                        sizes: '64x64',
                        type: 'image/png'
                    },
                    {
                        src: './pwa/128x128.png',
                        sizes: '128x128',
                        type: 'image/png'
                    },
                    {
                        src: './pwa/256x256.png',
                        sizes: '256x256',
                        type: 'image/png'
                    },
                    {
                        src: './pwa/512x5122.png',
                        sizes: '512x512',
                        type: 'image/png'
                    }
                ]
            }
        })
    ],
    resolve: {
        alias: [
            {find: '@', replacement: fileURLToPath(new URL('./src', import.meta.url))},
            {find: '@features', replacement: fileURLToPath(new URL('./src/features', import.meta.url))},
            {find: '@types', replacement: fileURLToPath(new URL('./src/types', import.meta.url))},
            {find: '@utils', replacement: fileURLToPath(new URL('./src/utils', import.meta.url))},
            {find: '@pages', replacement: fileURLToPath(new URL('./src/pages', import.meta.url))}
        ],
    },
    build: {
        outDir: '../presyotect-next/wwwroot',
        emptyOutDir: true
    }
})
