var fs = require('fs')

var stream = new fs.ReadStream(__filename, {encoding : 'utf-8'})
stream.on('readable', function() {
    var data = stream.read()
    console.log(data)
})
stream.on('end', function() {
    console.log("THE END")
})
stream.on('open', function() {
    console.log("Open")
})
stream.on('close', function() {
    console.log("Close")
})
stream.on('error', function(err) {
    console.error(err)
})