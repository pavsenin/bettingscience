var session = require('express-session')
var MongoStore = require('connect-mongo')(session);
var mongoose = require('./mongoose')

var sessionStore = new MongoStore({mongoose_connection: mongoose.connection});

module.exports = sessionStore;