# client
if `npm run serve` errors with 
```
options.clientLogLevel should be {String} and equal to one of the allowed values

 [ 'info', 'warn', 'error', 'debug', 'trace', 'silent' ]
```
in `\node_modules\@vue\cli-service\lib\commands\serve.js` change clientLogLevel to info instead of none.
## Project setup
```
npm install
```

### Compiles and hot-reloads for development
```
npm run serve
```

### Compiles and minifies for production
```
npm run build
```

### Run your tests
```
npm run test
```

### Lints and fixes files
```
npm run lint
```

### Run your unit tests
```
npm run test:unit
```

### Customize configuration
See [Configuration Reference](https://cli.vuejs.org/config/).
