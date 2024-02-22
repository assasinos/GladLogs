const PROXY_CONFIG = [
  {
    context: [
      "/api",
      "/api/chats",
      "/api/messages",
      "/api/messages/period/",
      "/api/user/"
    ],
    target: "https://localhost:7039",
    secure: false
  }
]

module.exports = PROXY_CONFIG;
