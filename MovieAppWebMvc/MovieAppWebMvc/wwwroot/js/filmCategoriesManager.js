class FilmCategoriesManager {
    constructor(apiBaseUrl) {
        this.apiBaseUrl = apiBaseUrl;
    }

    async getCategoriesByFilm(filmId) {
        try {
            const response = await fetch(`${this.apiBaseUrl}/filmCategories/${filmId}`);
            if (!response.ok) {
                throw new Error('Network not connected');
            }
            return await response.json();
        } catch (error) {
            console.error("Categories not found", error);
        }
    }

    async addCategoryToFilm(filmId, categoryId) {
        try {
            const response = await fetch(`${this.apiBaseUrl}/filmCategories`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ FilmId: filmId, CategoryId: categoryId })
            });
            if (!response.ok) {
                throw new Error('Network not connected');
            }
            return await response.json();
        } catch (error) {
            console.error("Category not added to film", error);
        }
    }

    async removeCategoryFromFilm(filmCategoryId) {
        try {
            const response = await fetch(`${this.apiBaseUrl}/filmCategories/${filmCategoryId}`, {
                method: 'DELETE'
            });
            if (!response.ok) {
                throw new Error('Network not connected');
            }
            return true;
        } catch (error) {
            console.error("Could not remove category from film:", error);
            return false;
        }
    }
}
