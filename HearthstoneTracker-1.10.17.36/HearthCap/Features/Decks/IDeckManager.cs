﻿using System;
using System.Collections.Generic;
using HearthCap.Data;

namespace HearthCap.Features.Decks
{
    public interface IDeckManager
    {
        IEnumerable<Deck> GetDecks(string server, bool includeDeleted = false);

        void ClearCache();

        void AddDeck(DeckModel deck);

        void UpdateDeck(DeckModel deck, bool suppressEvent = false);

        void DeleteDeck(Guid id);

        Deck GetOrCreateDeckBySlot(string server, string slot);

        Deck GetDeckById(Guid id);

        IEnumerable<Deck> GetAllDecks(bool includeDeleted = false);

        void UpdateDeckImage(Guid deckModel, byte[] image);

        void UndeleteDeck(Guid id);
    }
}
