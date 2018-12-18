var util = require('util')
function PhraseError(msg) {
    this.message = msg
    Error.captureStackTrace(this, PhraseError)
}
util.inherits(PhraseError, Error)
PhraseError.prototype.name = 'PhraseError'
function HttpError(st, msg) {
    this.status = st
    this.message = msg
}
util.inherits(HttpError, Error)
HttpError.prototype.name = 'HttpError'

var phrases = {
    "Hello":"Привет",
    "world":"мир"
}
function getPhrase(name) {
    if(!phrases[name])
        throw new PhraseError("Нет фразы " + name)
    return phrases[name]
}
function makePage(url) {
    if(url != 'index.html')
        throw new HttpError(404, "Нет такой страницы")
    return util.format("%s, %s!", getPhrase("Hell1o"), getPhrase("world"))
}
try {
    var page = makePage('index.html')
    console.log(page)
} catch(e) {
    if(e instanceof HttpError) {
        console.log(e.status, e.message)
    } else {
        console.error("%s %s %s", e.name, e.message, e.stack)
    }
}