const cacheName = "Vireye-ToTheMoon-0.1.82";
const contentToCache = [
    "Build/9853637125e801e9aae48e78dbbdcfca.loader.js",
    "Build/6115b8fff235cc1f1ba59adc6af5a476.framework.js.unityweb",
    "Build/86aa1be36c2378c81ea594f4009a4ac9.data.unityweb",
    "Build/f6f74c0acbda15644b831f9f5d6900d2.wasm.unityweb",
    "TemplateData/style.css"

];

self.addEventListener('install', function (e) {
    console.log('[Service Worker] Install');
    
	e.waitUntil(
		caches.keys().then((cacheNames) => {
		  return Promise.all(
			cacheNames
			  .filter((name) => {
				return name !== cacheName;
			  })
			  .map((name) => {
				console.log("[Service Worker] Deleting old cache:", name);
				return caches.delete(name);
			  })
		  );
		})
	  );
    e.waitUntil((async function () {
      const cache = await caches.open(cacheName);
      console.log('[Service Worker] Caching all: app shell and content');
      await cache.addAll(contentToCache);
    })());
});

self.addEventListener('fetch', function (e) {
    e.respondWith((async function () {
      let response = await caches.match(e.request);
      console.log(`[Service Worker] Fetching resource: ${e.request.url}`);
      if (response) { return response; }

      response = await fetch(e.request);
      const cache = await caches.open(cacheName);
      console.log(`[Service Worker] Caching new resource: ${e.request.url}`);
      cache.put(e.request, response.clone());
      return response;
    })());
});
