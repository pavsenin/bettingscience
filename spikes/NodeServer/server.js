var db = require('db')
var util = require('util')
var log = require('logger')(module)
var User = require('./user')

db.connect()

function run() {
    var obj = {a:5, b:true, go:function() {return 1}}
    console.log(util.format("XXX %j", obj))
    var vasya = new User("Вася")
    var petya = new User("Петя")
    

    vasya.hello(petya)
    log(db.getPhrase("Run successful"))
}

if(module.parent) {
    exports.run = run;
} else {
    run();
}