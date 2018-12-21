var http = require('http')
var fs = require('fs')


var server = http.createServer(function(req, res) {
    var info;
    if(req.url == '/') {
        fs.readFile('index.html', function(err, info) {
            if (err) {
                res.statusCode = 500
                res.end('Program error')
            }
            res.end(info)
        })
    } else if (req.url == '/now') {
        res.end(new Date().toString())
    }
}).listen(3000, '127.0.0.1')