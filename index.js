const express = require('express');

const app = express();
const PORT = 3000;

// Health check
app.get('/api/v1/health', (req, res) => {
  res.status(200).json({ status: 'OK' });
});

// Clima (simulado por enquanto)
app.get('/api/v1/clima/:cidade', (req, res) => {
  const cidade = req.params.cidade;

  res.status(200).json({
    cidade: cidade,
    temperatura: 30,
    descricao: "Ensolarado"
  });
});

app.listen(PORT, () => {
  console.log(`Servidor rodando em http://localhost:${PORT}`);
});