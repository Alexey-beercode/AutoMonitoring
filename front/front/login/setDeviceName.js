const fpPromise = import('./fp.min.js')
    .then(FingerprintJS => FingerprintJS.load())

fpPromise
    .then(fp => fp.get())
    .then(result => {
      // This is the visitor identifier:
      const visitorId = result.visitorId
      localStorage.setItem('deviceName', visitorId)
})