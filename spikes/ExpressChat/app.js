var express = require('express');
var http = require('http');
var path = require('path');
var config = require('./config');
var log = require('./libs/log')(module);
var errorHandler = require('errorhandler')
var session = require('express-session')
var connect = require('connect');
var cookie = require('cookie');
var cookieParser = require('cookie-parser')
var bodyParser = require('body-parser')
var HttpError = require('./error').HttpError

function loadSession(sid, callback) {
  sessionStore.load(sid, function(err, session) {
    if(arguments.length == 0) {
      return callback(null, null);
    } else {
      return callback(null, session);
    }
  })
}

function loadUser(session, callback) {
  if(!session.user) {
    return callback(null, null);
  }
  User.findById(session.user, function(err, user) {
    if (err) return callback(err);

    if (!user) return callback(null, null);

    callback(null, user);
  })
}

var app = express();

var server = http.createServer(app);
server.listen(config.get('port'), function() {
  log.info('Express listening port ' + config.get('port'))
});

var io = require('socket.io').listen(server);
io.set('origins', 'localhost:*');
io.set('logger', log);

io.set('authorization', function(handshake, callback) {
  async.waterfall([
    function(callback) {
      handshake.cookies = cookie.parse(handshake.headers.cookie || '');
      var sidCookie = handshake.cookies[config.get('session:key')];
      var sid = connect.utils.parseSignedCookie(sidCookie, config.get('session:secret'));

      loadSession(sid, callback);
    },
    function (session, callback) {
      if (!session) {
        callback(new HttpError(401, "No session"));
      }
      handshake.session = session;
      loadUser(session, callback);
    },
    function(user, callback) {
      if(!user)
        callback(new HttpError(403, "Anonymous session may not connect"));

      handshake.user = user;
      callback(null);
    }
  ], function(err) {
    if(!err)
      return callback(null, true);
    if (err instanceof HttpError)
      return callback(null, false);
    callback(err);
  })
})


io.sockets.on('session:reload', function(sid) {
  var clients = io.sockets.clients()
  clients.forEach(function(client) {
    if(client.handshake.session.id != sid) return;
    loadSession(sid, function(err, session) {
      if (err) {
        client.emit('error', 'server error');
        client.disconnect();
        return;
      }
      if (!session) {
        client.emit('logout');
        client.disconnect();
        return;
      }
      client.handshake.session = session;
    });
  })
});

io.sockets.on('connection', function(socket) {

  var username = socket.handshake.user.get('username');
  socket.broadcast.emit('join', username);

  socket.on('message', function(data, cb) {
    socket.broadcast.emit('message', username, data);
    cb && cb();
  });

  socket.on('disconnect', function() {
    socket.broadcast.emit('leave', username);
  });
})

app.set('io', io);

// view engine setup
app.engine('ejs', require('ejs-locals'));
app.set('views', path.join(__dirname, 'views'));
app.set('view engine', 'ejs');

app.use(express.json());
app.use(express.urlencoded({ extended: false }));
app.use(bodyParser());
app.use(cookieParser());

var sessionStore = require('./libs/sessionStore');
app.use(session({
  secret: config.get('session:secret'),
  //key: config.get('session:key'),
  cookie: config.get('session:cookie'),
  store: sessionStore
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
