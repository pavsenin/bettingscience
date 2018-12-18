var http = require('http')
var url = require('url')

var server = new http.Server()
server.listen(1337, '127.0.0.1')

var cnt = 0
var emit = server.emit
server.emit = function(event) {
    console.log(event)
    emit.apply(server, arguments)
}
server.on('request', function(req, res) {
    console.log(req.headers)

    var urlParsed = url.parse(req.url, true)
    if(urlParsed.pathname == '/echo' && urlParsed.query.message) {
        res.setHeader('Cache-control', 'no-cache')
        res.end(urlParsed.query.message)
    } else {
        res.statusCode = 404
        res.end('Page not found')
    }
})