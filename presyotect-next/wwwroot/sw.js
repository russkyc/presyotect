if(!self.define){let e,i={};const n=(n,r)=>(n=new URL(n+".js",r).href,i[n]||new Promise((i=>{if("document"in self){const e=document.createElement("script");e.src=n,e.onload=i,document.head.appendChild(e)}else e=n,importScripts(n),i()})).then((()=>{let e=i[n];if(!e)throw new Error(`Module ${n} didn’t register its module`);return e})));self.define=(r,s)=>{const a=e||("document"in self?document.currentScript.src:"")||location.href;if(i[a])return;let o={};const c=e=>n(e,a),l={module:{uri:a},exports:o,require:c};i[a]=Promise.all(r.map((e=>l[e]||c(e)))).then((e=>(s(...e),o)))}}define(["./workbox-3e8df8c8"],(function(e){"use strict";self.skipWaiting(),e.clientsClaim(),e.precacheAndRoute([{url:"assets/index-CpI4VFhM.js",revision:null},{url:"assets/index-DKAvY2ll.css",revision:null},{url:"assets/primeicons-Dr5RGzOO.svg",revision:null},{url:"branding/login-wallpaper.jpg",revision:"e7a1578bc690bac559ae2afbba46fd48"},{url:"branding/logo-horizontal.svg",revision:"16355c9e66123b422523c9e25cbd278d"},{url:"branding/presyotect-icon.png",revision:"bba34fa83493dc7884981e4fe9e9a126"},{url:"index.html",revision:"42b3057b2a3aafaa5156590e1379436c"},{url:"presyotect-icon.png",revision:"bba34fa83493dc7884981e4fe9e9a126"},{url:"pwa/128x128.png",revision:"942b447376623b0d7f6491b879bc7095"},{url:"pwa/256x256.png",revision:"d2cc69056c17d2f81ea641921b5cef07"},{url:"pwa/512x512.png",revision:"5465087574f1a74781c002b259bdd493"},{url:"pwa/64x64.png",revision:"5f2704ad5196016013ea1bc52a6a943d"},{url:"registerSW.js",revision:"1872c500de691dce40960bb85481de07"},{url:"vite.svg",revision:"8e3a10e157f75ada21ab742c022d5430"},{url:"pwa/128x128.png",revision:"942b447376623b0d7f6491b879bc7095"},{url:"pwa/256x256.png",revision:"d2cc69056c17d2f81ea641921b5cef07"},{url:"pwa/64x64.png",revision:"5f2704ad5196016013ea1bc52a6a943d"},{url:"manifest.webmanifest",revision:"2c715f984d2219935a382693ff62a05b"}],{}),e.cleanupOutdatedCaches(),e.registerRoute(new e.NavigationRoute(e.createHandlerBoundToURL("index.html"),{denylist:[/^\/_api/,/^\/_api_docs/]}))}));
//# sourceMappingURL=sw.js.map
