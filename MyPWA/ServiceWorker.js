const cacheName = '"AnroiDevel-MultiTool-1.0"';
const contentToCache = [
    "Build/40b0f14b1efdf376baccf14d6c0ef188.loader.js",
    "Build/11a65cc0e8628c0a9edd1220c2afc1f4.framework.js.unityweb",
    "Build/093597b1ebb720c8940689b4ff3a4af9.data.unityweb",
    "Build/b53f1dba378d07422905281ac6a6502e.wasm.unityweb",
    "TemplateData/style.css"
];

self.addEventListener('install', function (e) {
    console.log('[Service Worker] Install');
    e.waitUntil(
        caches.open(cacheName).then(function (cache) {
            console.log('[Service Worker] Caching all: app shell and content');
            return cache.addAll(contentToCache);
        })
    );
});

self.addEventListener('activate', function (e) {
    e.waitUntil(
        caches.keys().then(function (cacheNames) {
            return Promise.all(
                cacheNames.map(function (thisCacheName) {
                    if (thisCacheName !== cacheName) {
                        console.log('[Service Worker] Deleting old cache:', thisCacheName);
                        return caches.delete(thisCacheName);
                    }
                })
            );
        })
    );
    return self.clients.claim();
});

self.addEventListener('fetch', function (e) {
    e.respondWith(
        caches.open(cacheName).then(async function (cache) {
            const response = await fetch(e.request);
            if (response.status === 200 && response.type === 'basic') {
                console.log(`[Service Worker] Caching new resource: ${e.request.url}`);
                cache.put(e.request, response.clone());
            }
            return response;
        }).catch(function () {
            return caches.match(e.request);
        })
    );
});
