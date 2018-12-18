var EventEmitter = require('events').EventEmitter

var server = new EventEmitter

server.on('request', function(req) {
    req.approved = true
})
server.on('request', function(req) {
    console.log(req)
})
server.on('error', function(error) {
    //console.error(error)
})
server.emit('request', {from : "Клиент"})
server.emit('request', {from : "Еще клиент"})
server.emit('error', new Error('eee'))