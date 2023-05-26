// const functions = require("firebase-functions");

const express = require("express");
const app = express();

const dotenv = require('dotenv');
dotenv.config();
const bodyParser = require('body-parser')
app.use(bodyParser.urlencoded({
  extended: true
}));
app.use(bodyParser.json());

const cors = require('cors');
app.use(cors());


const Route = require('./routes/index');
Route(app);

const port = process.env.PORT || 5500;
app.listen(port,() => console.log(`http://localhost:${port}`));

module.exports = app;

// exports.app = functions.https.onRequest(app);