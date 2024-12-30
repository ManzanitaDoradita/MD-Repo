version = 1.1

class PlayHubLite : MainAPI() {
    override val mainUrl = "https://playhublite.com"
    override val name = "PlayHubLite"
    override val lang = "es"

    override val searchUrl = "$mainUrl/search?query=%s"

    override suspend fun search(query: String): List<SearchResponse> {
        val doc = app.get(searchUrl.format(query)).document
        return doc.select(".video-card").map {
            val title = it.selectFirst(".title")?.text() ?: ""
            val href = it.selectFirst("a")?.attr("href") ?: ""
            val poster = it.selectFirst("img")?.attr("src") ?: ""
            newMovieSearchResponse(title, href) {
                this.posterUrl = poster
            }
        }
    }

    override suspend fun load(url: String): LoadResponse {
        val doc = app.get(url).document
        val videoUrl = doc.selectFirst("iframe")?.attr("src") ?: ""
        return newMovieLoadResponse(doc.title(), url) {
            addLink(videoUrl)
        }
    }
}