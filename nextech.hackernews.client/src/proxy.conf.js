const { env } = require('process');

const target = env["services__nextech.hackernews-server__https__0"] ?? 'https://localhost:7135';

const PROXY_CONFIG = [
  {
    context: [
      "/weatherforecast",
    ],
    target,
    secure: false
  }
]

module.exports = PROXY_CONFIG;
