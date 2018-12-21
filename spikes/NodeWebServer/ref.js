var http = require('http')
var fs = require('fs')


var server = http.createServer(function(req, res) {
    process.nextTick(function() {
        
    }, 0)
}).listen(3000, '127.0.0.1')

setTimeout(function() {
    server.close()
}, 2500)

var timer = setInterval(function() {
    console.log(process.memoryUsage())
}, 1000)
timer.unref()