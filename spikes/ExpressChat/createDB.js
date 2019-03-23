var mongoose = require('./libs/mongoose')
var async = require('async')

async.series([
    open,
    dropDatabase,
    requireModels,
    createUsers
], function(err, results) {
    console.log(arguments)
    close(null)
})

function open(callback) {
    mongoose.connection.on('open', callback)
}
function dropDatabase(callback) {
    var db = mongoose.connection.db
    db.dropDatabase(callback)
}
function requireModels(callback) {
    require('./models/user')

    async.each(Object.keys(mongoose.models), function(modelName, callback) {
        mongoose.models[modelName].ensureIndexes(callback)
    }, callback)
}
function createUsers(callback) {
    var users = [
        {username: 'Толя', password: 'Шарик'},
        {username: 'Петя', password: 'Порро'},
        {username: 'Вова', password: 'Клоун'}
    ]
    async.each(users, function(userData, callback) {
        var user = new mongoose.models.User(userData)
        user.save(callback)
    }, callback)
}
function close(callback) {
    mongoose.disconnect(callback)
}


// var User = require('./models/user').User;

// var user = new User({
//     username: "Tester2",
//     password: 'secret'
// })

// user.save(function(err, user, affected) {
//     if (err) throw err;

//     User.findOne({username:"Tester"}, function(err, tester) {
//         console.log(tester);
//     })
// });

// var mongoose = require('mongoose');
// mongoose.connect('mongodb://localhost/test');

// var schema = mongoose.Schema({
//     name:String
// })
// schema.methods.meow = function() {
//     console.log(this.get('name'));
// }

// var Cat = mongoose.model('Cat', schema);
// var kitty = new Cat({name:'Zildjian'});
// kitty.save(function(err, kitty, affected){
//     kitty.meow();
// })



// var MongoClient = require('mongodb').MongoClient
//     , format = require('util').format;

// MongoClient.connect(
//     'mongodb://127.0.0.1:27017/chat', function(err, db) {
//         if(err) throw err;
//         var collection = db.collection('test_insert');
//         collection.remove({}, function(err, affected) {
//             if(err) throw err;
//             collection.insert({a:2}, function(err, docs) {
//                 var cursor = collection.find({a:2});
//                 cursor.toArray(function(err, results) {
//                     console.dir(results);
//                     db.close();
//                 })
//             })
//         });
//     }
// );