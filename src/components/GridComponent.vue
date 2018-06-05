<template>
  <div>
    <v-card>
      <v-container grid-list-md>
        <v-card-title >
          <v-text-field v-model="search"
                        append-icon="search"
                        label="Search"
                        single-line
                        hide-details>
          </v-text-field>

          <!-- <v-dialog v-model="dialog"
                    max-width="500px">
            <v-btn icon
                  slot="activator"
                  class="mb-2">
              <v-icon color="teal">
                add
              </v-icon>
            </v-btn>
          </v-dialog> -->
        </v-card-title>

        <v-data-table :headers="headers"
                      :items="items"
                      class="elevation-1">
          <template slot="items" slot-scope="props">
            <td v-for="(header, i) in headers" :key="i">
              {{ props.item[header.value] }}
            </td>

            <td slot="activator">
              <v-btn v-if="showEdit" icon class="mx-0" @click="editItem(props.item)">
                <v-icon color="teal">edit</v-icon>
              </v-btn>

              <v-btn v-if="showDelete" icon class="mx-0" @click="deleteItem(props.item)">
                <v-icon color="pink">delete</v-icon>
              </v-btn>
            </td>
            <template slot="no-data">
              <v-btn color="primary">Reset</v-btn>
            </template>
          </template>
        </v-data-table>
      </v-container>
    </v-card>
  </div>
</template>

<script>


export default {
  props: {
    items: {
      type: Array,
      required: false,
      default() {
        return [];
      }
    },
    headers: {
      type: Array,
      required: false,
      default() {
        return [];
      }
    },
    editedItem: {
      type: Object,
      required: false,
      default() {
        return {};
      }
    },
    showEdit: {
      type: Boolean,
      required: false
    },
    showDelete: {
      type: Boolean,
      required: false
    }
  },

  data() {
    return {
      search: '',
      showEditPageComponent: false,
      editedIndex: -1
    };
  },

  methods: {
    editItem(item) {
      // this.editedIndex = this.items.indexOf(item);
      // this.editedItem = Object.assign({}, item);
      // this.showEditPageComponent = true;

      this.$emit('edited', item);
    },

    deleteItem(item) {
      const index = this.items.indexOf(item);

      confirm("Are you sure you want to delete this item?") &&
        this.items.splice(index, 1);
    }
  }
};
</script>
