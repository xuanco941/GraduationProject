const Features = require('./Features');



function Route(app) {
    app.use('/api', Features);
}


module.exports = Route