const express = require('express');
const bodyParser = require('body-parser');
const cors = require('cors');
const mysql = require('mysql');

const app = express();
const port = 3000;

app.use(cors());
app.use(bodyParser.json());

app.use(bodyParser.json({ limit: '50mb' }));
app.use(bodyParser.urlencoded({ limit: '50mb', extended: true }));

const db = mysql.createConnection({
    host: 'localhost',
    user: 'root',
    password: '',
    database: 'dbpm02'
});

db.connect((err) => {
    if(err){
        console.log('Error no esta conectado a la base de datos');
        return;
    }
    console.log('Conectado a la base de datos')
});

//Create
app.post('/api/sitios', (req, res) => {
    const {descripcion, latitud, longitud, firma, audio} = req.body;
    const consulta = 'INSERT INTO SITIOS (descripcion, latitud, longitud, firma, audio) VALUES (?, ?, ?, ?, ?)';

    db.query(consulta, [descripcion, latitud, longitud, firma, audio], (err,result) => {
        if(err){
            res.status(500).send(err);
            return;
        }
        res.status(200).send(result);
    });
});

//Read
app.get('/api/sitios',(req,res) => {
    const consulta = 'SELECT * FROM sitios';

    db.query(consulta,(err,resultado) => {
        if(err){
            res.status(500).send(err);
            return;
        }
        res.status(200).send(resultado);
    });
});

//Update
app.put('/api/sitios/:id', (req, res) => {
    const {id} = req.params;
    const {descripcion, latitud, longitud, firma, audio} = req.body;

    const consulta = 'UPDATE sitios SET descripcion = ?, latitud = ?, longitud = ?, firma = ?, audio = ? WHERE id = ?';

    db.query(consulta, [descripcion, latitud, longitud, firma, audio, id], (err, result) => {
        if (err) {
            res.status(500).send(err);
            return;
        }
        res.status(200).send(result);
    });
});

//Delete
app.delete('/api/sitios/:id', (req, res) => {
    const { id } = req.params;
    const query = 'DELETE FROM sitios WHERE id = ?';
    db.query(query, [id], (err, result) => {
      if (err) {
        res.status(500).send(err);
        return;
      }
      res.status(200).send(result);
    });
  });


app.listen(port, ()=> {
    console.log(`Servidor ejecutandose en http://192.168.100.29:${port}`);
});