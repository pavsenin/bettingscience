var express = require('express');
var http = require('http');
var path = require('path');
var config = require('./config');
var log = require('./libs/log')(module);
var errorHandler = require('errorhandler')
var session = require('express-session')
var cookieParser = require('cookie-parser')
var bodyParser = require('body-parser')
var HttpError = require('./error').HttpError
var mongoose = require('./libs/mongoose')


var app = express();

var server = http.createServer(app);
server.listen(config.get('port'), function() {
  log.info('Express listening port ' + config.get('port'))
});

var io = require('socket.io').listen(server);
io.set('origins', 'localhost:*');
io.set('logger', log);
io.sockets.on('connection', function(socket) {
  socket.on('message', function(data, cb) {
    console.log(data);
    socket.broadcast.emit('message', data);
    cb(data);
  });
})

// view engine setup
app.engine('ejs', require('ejs-locals'));
app.set('views', path.join(__dirname, 'views'));
app.set('view engine', 'ejs');

app.use(express.json());
app.use(express.urlencoded({ extended: false }));
app.use(bodyParser());
app.use(cookieParser());

var MongoStore = require('connect-mongo')(session)
app.use(session({
  secret: config.get('session:secret'),
  //key: config.get('session:key'),
  cookie: config.get('session:cookie'),
  store: new MongoStore({mongooseConnection: mongoose.connection})
}))

// app.use(function(req, res, next) {
//   req.session.numberOfVisits = req.session.numberOfVisits + 1 || 1
//   res.send("Visits: " + req.session.numberOfVisits);
// })

app.use(express.static(path.join(__dirname, 'public')));

app.use(require('./middleware/sendHttpError'))
app.use(require('./middleware/loadUser'))

require('./routes/index')(app)

// error handler
//app.use(errorHandler());
app.use(function(err, req, res, next) {
  if (typeof err == 'number') {
    err = new HttpError(err)
  }
  if (err instanceof HttpError) {
    res.sendHttpError(err)
  } else {
    if (app.get('env') === 'development') {
      errorHandler()(err, req, res, next)
    } else {
      err = new HttpError(500)
      res.sendHttpError(err)
    }
  }
});

module.exports = app;
